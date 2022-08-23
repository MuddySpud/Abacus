using System;

namespace MuddySpud.RulesEngine
{
    public class FileStructureFactory
    {
        public IFileStructureInterpreter Build(FileStructureSettings fileStructureSettings)
        {
            switch (fileStructureSettings.Type)
            {
                case "ClassMethodStructure":
                    return new CSharpStructureInterpreter();

                default:
                    throw new NotImplementedException($"Code not written for type: {fileStructureSettings.Type}");
            }
        }
    }
}
