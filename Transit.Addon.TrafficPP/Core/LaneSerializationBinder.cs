﻿using System;
using System.Runtime.Serialization;

namespace Transit.Addon.TrafficPP.Core
{
    public class LaneSerializationBinder : SerializationBinder
    {
        public override Type BindToType(string assemblyName, string typeName)
        {
            if (typeName.Contains("Lane"))
                return typeof(Lane);
            if (typeName.Contains("VehicleType"))
                return typeof(VehicleTypePP);

            throw new SerializationException("Error on BindToType with type '" + typeName + "' and assembly '" + assemblyName + "'.");
        }
    }
}