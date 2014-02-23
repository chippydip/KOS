﻿using System.Collections.Generic;

namespace kOS.Suffixed.Part
{
    public class DockingPortValue: PartValue
    {
        private readonly ModuleDockingNode module;

        public DockingPortValue(global::Part part, ModuleDockingNode module) : base(part)
        {
            this.module = module;
        }

        public override object GetSuffix(string suffixName)
        {
            switch (suffixName)
            {
                case "STATE":
                    return module.state;
                case "ORIENTATION":
                    return new Vector( module.GetFwdVector() );
                case "DOCKEDVESSELNAME":
                    return module.vesselInfo.name;
                case "TARGETABLE":
                    return true;
            }
            return base.GetSuffix(suffixName);
        }

        public override bool SetSuffix(string suffixName, object value)
        {
            switch (suffixName)
            {
                case "CONTROLFROM":
                    {
                        var control = (bool) value;
                        if (control)
                        {
                            module.MakeReferenceTransform();
                        }
                        else
                        {
                            module.vessel.SetReferenceTransform(module.vessel.rootPart);
                        }
                        break;
                    }
                    
            }
            return base.SetSuffix(suffixName, value);
        }

        public override ITargetable Target
        {
            get { return module; }
        }

        public new static ListValue PartsToList(IEnumerable<global::Part> parts)
        {
            var toReturn = new ListValue();
            foreach (var part in parts)
            {
                foreach (PartModule module in part.Modules)
                {
                    var dockingNode = module as ModuleDockingNode;
                    if (dockingNode != null)
                    {
                        toReturn.Add(new DockingPortValue(part, dockingNode));
                    }
                }
            }
            return toReturn;
        }
    }
}
