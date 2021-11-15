using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace libProChic
{
  public  class ConfigHelper
    {
        private List<ConfigGroup> winINI { get; set; } = new List<ConfigGroup>();            //Variable that will hold each config line
        private String fileLoc { get; set; } = "";         //File Location of the Config File   
        FileSystemWatcher fsw;       //Creates FileWatcher to update file as needed
        public ConfigHelper(String fileName):this(fileName,true) {        }
        public ConfigHelper(string fileName,Boolean autoUpdate) {
            Debug.WriteLine(fileName);
            fileLoc = fileName;            //If does then Saves the config file location
            if (File.Exists(fileName)){
                updateConfig();     //Adds contents of config to WinINI if fileName exist
                fsw = new FileSystemWatcher(Path.GetDirectoryName(fileName));
                fsw.EnableRaisingEvents = true;     //Enables FSW to raise events
                AutoRefresh = autoUpdate;
            }
        }
        private void fsw_Changed(object sender, FileSystemEventArgs e){
          if (auto)  updateConfig(); //When Config is updated externally, then update Config
         if (ConfigUpdated !=null)   ConfigUpdated(sender, e);  //If possible raise ConfigUpdated so that other classes can update needed components
        }
        private Boolean auto = false;
        public Boolean AutoRefresh { get { return auto; } set { auto = value; if (auto) fsw.Changed += fsw_Changed; else fsw.Changed -= fsw_Changed; } }     //Adds/Removes method to be exe when event is raised } }
        public String[] ReadAllLines(String filepath) {   //Method that reads a file even if another process is also reading it and returns it as an Array
            try {
                FileStream logFileStream = new FileStream(filepath, FileMode.OpenOrCreate, FileAccess.Read, FileShare.ReadWrite);      //If file doesn't exist, then create it or just open it
                StreamReader logFileReader = new StreamReader(logFileStream);
                List<String> line = new List<string>();
                while (!logFileReader.EndOfStream){
                    line.Add(logFileReader.ReadLine());
                }
                logFileReader.Close();
                logFileStream.Close();
                return line.ToArray();
            } catch(Exception e) {
                throw new Exception("File can't be created/n" +e.ToString());       //Errors out if folder isn't created or doesn't have permissions
            }
        }
        private void updateConfig(){
            String[] fil = ReadAllLines(fileLoc);     //Reads config file and places it in String varible fil
            winINI.Clear();     //Clears any items that are currently in the list
            for (int i = 0; i <= fil.Length - 1; i++)      //Loops through each line of the fil
            {
                if (fil[i].StartsWith("[") && fil[i].EndsWith("]"))
                {     //Checks to see if line is a group header  
                    ConfigGroup grp = new ConfigGroup(fil[i], i);       //Creates a new group and sets the name and index of header
                    i++;    //Increments down to first line in group
                    for(bool b=false; i < fil.Length && fil[i] != "";){      //Loops through each config in group, need for loop for pretest loop, j won't be used, but is needed for the loop  Loops through till blank line or end of array                    
                        grp.Add(new Config(fil[i]));    //Creates config out of line and adds it to grp
                        i++;        //Increments to next line of String
                    }
                    grp.Index = winINI.Count;   //Adds Index, found by the count of the previous amount in winINI
                    winINI.Add(grp);        //Adds grp to winINI (which holds the complete config file)
                }
            }
        }
        public event EventHandler ConfigUpdated;
        public ConfigGroup GetConfigGroup(string group2Find){
            foreach(ConfigGroup grp in winINI){     //Loops through each ConfigGroup
               // Console.WriteLine(grp.Name);
                if (grp.Name.Equals(group2Find,StringComparison.CurrentCultureIgnoreCase)) return grp;     //If the group name matches the one being searched  for then return it
            }
            throw new Exception($"ConfigGroup: {group2Find} not Found");     //If the group was not found then throw exception
        }
        public ConfigGroup[] GetAllConfigsGroup()
        {
            return winINI.ToArray();            
        }
        public void RemoveGroup(string Group2Find){
            winINI.RemoveAt(GetConfigGroup(Group2Find).Index);
            for (int index = 0; index <= winINI.Count - 1; index++){        //Loops through each configGroup of winINI
                if (winINI[index].Name==Group2Find){        //If element equals search
                    winINI.RemoveAt(index);                 //The ConfigGroup is removed
                    FileChanged = true;                       //Boolean that identifies that there has been a change is set to true (LEGACY)
                }
            }
        }
        public void RemoveConfig(String configGroup, string Config2Find){
            GetConfigGroup(configGroup).Remove(Config2Find);        //Finds group and removes config specified in String
        }
        public void AddGroup(String groupName){
            winINI.Add(new ConfigGroup(groupName)); //Adds new Config Group and adds it to winINI
        }
        public String FileLocation { get { return fileLoc; } }
        private Boolean FileChanged{
            set{
                File.WriteAllText(fileLoc, prepareFile());
            }
        }
        public   void Save(){
            File.WriteAllText(fileLoc, prepareFile());
        }
        public void SetConfig(String group, String configName, String newValue){
            GetConfigGroup(group).Item(configName).Setting = newValue;      //Finds Group, then Config and sets new value
        }
        public void SetConfig(String group, String configName, String newValue,Boolean createIfNotExist)
        {
            if (createIfNotExist && !Exists(group)) AddGroup(group);
            ConfigGroup grp = GetConfigGroup(group);    //Finds group
            if (createIfNotExist && grp.Contains(configName) == -1) grp.Add(new Config(configName+"="+newValue));else  grp.Item(configName).Setting = newValue;      //Finds Config (or creates it), and sets new value
        }
        public Config GetConfig(string group, string value2Find){
            return GetConfigGroup(group).Item(value2Find);
        }
        public Int32 GetConfigAsInt32(string group, string value2Find)
        {
            Int32 i = -1;
            Config c =GetConfigGroup(group).Item(value2Find);
            if (!Int32.TryParse(c.Setting, out i)) throw new Exception("Setting isn't an int32");
            return i;
        }
        private String prepareFile(){
            String cpl = "";    //Creates empty string that will be used to create assembled config file
            foreach (ConfigGroup grp in winINI){        //Loops through each ConfigGroup in winINI
                cpl += $"[{grp.Name}] {Environment.NewLine}";   //Places Group Heading in String
                foreach(Config con in grp.ToArray()){       ///Loops through each config in Group
                    cpl += $"{con.SettingName}={con.Setting}{Environment.NewLine}"; //Concats setting line
                }
                cpl += Environment.NewLine; //Adds a empty line between Groups
            }
            cpl.Trim();     //Removes the last NewLine
            return cpl;     //Returns compiled file
        }
        public Boolean Exists(String GroupName){
            foreach(ConfigGroup c in winINI){
                if (c.Name.Equals(GroupName,StringComparison.CurrentCultureIgnoreCase)) return true;
            }
            return false;
        }
        private bool disposedValue = false; // To detect redundant calls
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                }

                // TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
                // TODO: set large fields to null.
                winINI = null;
            }
            this.disposedValue = true;
        }
        // TODO: override Finalize() only if Dispose(ByVal disposing As Boolean) above has code to free unmanaged resources.
        // Protected Overrides Sub Finalize()
        // ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        // Dispose(False)
        // MyBase.Finalize()
        // End Sub

        // This code added by Visual Basic to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        public void MoveFile(String newLocation)
        {
            File.Move(fileLoc, newLocation);
            fileLoc = newLocation;
        }
    }
    public class Config
    { public Config()
        {
        }
        public Config(string setName, string val) : this(setName + "=" + val)
        {        }
        public Config(string settingLine)
        {
            if (!settingLine.Contains("=")) {
                Console.WriteLine(settingLine);
                throw new Exception("Setting Line is not format correctly");
            }
            else
            {
                String[] set = settingLine.Split('=');
                SettingName = set[0];
                Setting = set[1];                
            }
        }
        public string SettingName { get; set; } = "";
        public string Setting { get; set; } = "";
        public string ConfigGroup { get; set; } = "";
        public int Index { get; set; } = -1;
        public override string ToString()
        {
            return SettingName + "=" + Setting;
        }
    }
    public class ConfigGroup : IEnumerator, IEnumerable
    {
        private int headerIndex = 0, pos=0;
        public String Name;
        public int Index { get; set; }
        public object Current{
            get{
                return Configs[pos];
            }
        }
        private List<Config> Configs = new List<Config>();
        public ConfigGroup(String groupName, int lineIndex):this(groupName){
            headerIndex = lineIndex;   
        }
        public ConfigGroup(String groupName){
            if (groupName.StartsWith("[")) groupName = groupName.Substring(1);
            if (groupName.EndsWith("]")) groupName=groupName.Substring(0,groupName.Length-1);
            Name = groupName;
        }
        public int Contains(String settingName){
            foreach(Config con in Configs) {      //Loops through each config in group
                if (con.SettingName == settingName){        //If config name matches the one being searched for
                    return con.Index;           //then the config's index is returned
                }
            }
            return -1;      //If Setting's name can't be found then -1 is retuned
        }
        public void Add(Config cfg){
            if (cfg.Index == -1) cfg.Index = Configs.Count;
            Configs.Add(cfg);       //Adds new Config to Group

        }
        public void RemoveAt(int index){
            Configs.RemoveAt(index);       //Removes config at index
        }
        public void Remove(String config2Find){
            try{
                Configs.RemoveAt(Contains(config2Find));    //Removes config by looking up the existence and index
            }catch{
                throw new Exception("Config not found");     //If it errors, which should happen if Contains(String) returns -1
            }            
        }
        public Config Item(int Index){
            try{
                return Configs[Index];      //Returns Item at specified Index
            }
            catch {
                throw new Exception("Index out of Range");  //If Errors, which happens when Index out of range error is returned
            }
        }
        public Config Item(String configName){
            try
            {
                return Configs[Contains(configName)];      //Returns Item at returned index of Contains(String)
            }
            catch
            {
                throw new Exception("Config Name not found: " + Name + ':' + configName);  //If Errors, which happens when configName doesn't exist then error is thrown
            }
        }
        public Config[] ToArray(){
            return Configs.ToArray();       //Returns an array of the whole group
        }
        public bool MoveNext(){
            pos++;
            return (pos < Configs.Count);
        }
        public void Reset(){
            pos = 0;
        }
        public IEnumerator GetEnumerator(){
            return (IEnumerator)this;
        }
    }
}