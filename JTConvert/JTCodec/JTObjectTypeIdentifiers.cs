﻿namespace JTConvert.JTCodec
{
    internal static class JTObjectTypeIdentifiers
    {

        public static readonly Dictionary<Type, GUID> ObjectTypeIdentifiers = new()
        {
            { typeof(JTNullElement),                        new(0xffffffff, 0xffff, 0xffff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff) },
            { typeof(JTBaseNodeElement),                    new(0x10dd1035, 0x2ac8, 0x11d1, 0x9b, 0x6b, 0x00, 0x80, 0xc7, 0xbb, 0x59, 0x97) },
            { typeof(JTGroupNodeElement),                   new(0x10dd101b, 0x2ac8, 0x11d1, 0x9b, 0x6b, 0x00, 0x80, 0xc7, 0xbb, 0x59, 0x97) },
            { typeof(JTInstanceNodeElement),                new(0x10dd102a, 0x2ac8, 0x11d1, 0x9b, 0x6b, 0x00, 0x80, 0xc7, 0xbb, 0x59, 0x97) },
            { typeof(JTLODNodeElement),                     new(0x10dd102c, 0x2ac8, 0x11d1, 0x9b, 0x6b, 0x00, 0x80, 0xc7, 0xbb, 0x59, 0x97) },
            { typeof(JTMetaDataNodeElement),                new(0xce357245, 0x38fb, 0x11d1, 0xa5, 0x6, 0x0, 0x60, 0x97, 0xbd, 0xc6, 0xe1) },
            { typeof(JTNullShapeNodeElement),               new(0xd239e7b6, 0xdd77, 0x4289, 0xa0, 0x7d, 0xb0, 0xee, 0x79, 0xf7, 0x94, 0x94) },
            { typeof(JTPartNodeElement),                    new(0xce357244, 0x38fb, 0x11d1, 0xa5, 0x6, 0x0, 0x60, 0x97, 0xbd, 0xc6, 0xe1) },
            { typeof(JTPartitionNodeElement),               new(0x10dd103e, 0x2ac8, 0x11d1, 0x9b, 0x6b, 0x00, 0x80, 0xc7, 0xbb, 0x59, 0x97) },
            { typeof(JTRangeLODNodeElement),                new(0x10dd104c, 0x2ac8, 0x11d1, 0x9b, 0x6b, 0x00, 0x80, 0xc7, 0xbb, 0x59, 0x97) },
            { typeof(JTSwitchNodeElement),                  new(0x10dd10f3, 0x2ac8, 0x11d1, 0x9b, 0x6b, 0x00, 0x80, 0xc7, 0xbb, 0x59, 0x97) },
            { typeof(JTBaseShapeNodeElement),               new(0x10dd1059, 0x2ac8, 0x11d1, 0x9b, 0x6b, 0x00, 0x80, 0xc7, 0xbb, 0x59, 0x97) },
            { typeof(JTPointSetShapeNodeElement),           new(0x98134716, 0x0010, 0x0818, 0x19, 0x98, 0x08, 0x00, 0x09, 0x83, 0x5d, 0x5a) },
            { typeof(JTPolygonSetShapeNodeElement),         new(0x10dd1048, 0x2ac8, 0x11d1, 0x9b, 0x6b, 0x00, 0x80, 0xc7, 0xbb, 0x59, 0x97) },
            { typeof(JTPolylineSetShapeNodeElement),        new(0x10dd1046, 0x2ac8, 0x11d1, 0x9b, 0x6b, 0x00, 0x80, 0xc7, 0xbb, 0x59, 0x97) },
            { typeof(JTPrimitiveSetShapeNodeElement),       new(0xe40373c1, 0x1ad9, 0x11d3, 0x9d, 0xaf, 0x0, 0xa0, 0xc9, 0xc7, 0xdd, 0xc2) },
            { typeof(JTTriStripSetShapeNodeElement),        new(0x10dd1077, 0x2ac8, 0x11d1, 0x9b, 0x6b, 0x00, 0x80, 0xc7, 0xbb, 0x59, 0x97) },
            { typeof(JTVertexShapeNodeElement),             new(0x10dd107f, 0x2ac8, 0x11d1, 0x9b, 0x6b, 0x00, 0x80, 0xc7, 0xbb, 0x59, 0x97) },
            { typeof(JTBaseAttributeData),                  new(0x10dd1001, 0x2ac8, 0x11d1, 0x9b, 0x6b, 0x00, 0x80, 0xc7, 0xbb, 0x59, 0x97) },
            { typeof(JTDrawStyleAttributeElement),          new(0x10dd1014, 0x2ac8, 0x11d1, 0x9b, 0x6b, 0x00, 0x80, 0xc7, 0xbb, 0x59, 0x97) },
            { typeof(JTGeometricTransformAttributeElement), new(0x10dd1083, 0x2ac8, 0x11d1, 0x9b, 0x6b, 0x00, 0x80, 0xc7, 0xbb, 0x59, 0x97) },
            { typeof(JTInfiniteLightAttributeElement),      new(0x10dd1028, 0x2ac8, 0x11d1, 0x9b, 0x6b, 0x00, 0x80, 0xc7, 0xbb, 0x59, 0x97) },
            { typeof(JTLightSetAttributeElement),           new(0x10dd1096, 0x2ac8, 0x11d1, 0x9b, 0x6b, 0x00, 0x80, 0xc7, 0xbb, 0x59, 0x97) },
            { typeof(JTLinestyleAttributeElement),          new(0x10dd10c4, 0x2ac8, 0x11d1, 0x9b, 0x6b, 0x00, 0x80, 0xc7, 0xbb, 0x59, 0x97) },
            { typeof(JTMaterialAttributeElement),           new(0x10dd1030, 0x2ac8, 0x11d1, 0x9b, 0x6b, 0x00, 0x80, 0xc7, 0xbb, 0x59, 0x97) },
            { typeof(JTPointLightAttributeElement),         new(0x10dd1045, 0x2ac8, 0x11d1, 0x9b, 0x6b, 0x00, 0x80, 0xc7, 0xbb, 0x59, 0x97) },
            { typeof(JTPointstyleAttributeElement),         new(0x8d57c010, 0xe5cb, 0x11d4, 0x84, 0xe, 0x00, 0xa0, 0xd2, 0x18, 0x2f, 0x9d) },
            { typeof(JTImageAttributeElement),              new(0x10dd1073, 0x2ac8, 0x11d1, 0x9b, 0x6b, 0x00, 0x80, 0xc7, 0xbb, 0x59, 0x97) },
            //{ typeof(JTTexCoordGenParameters),              new(0xaa1b831d, 0x6e47, 0x4fee, 0xa8, 0x65, 0xcd, 0x7e, 0x1f, 0x2f, 0x39, 0xdc) },
            { typeof(JTPaletteMapAttributeElement),         new(0x10dd1106,0x2ac8, 0x11d1, 0x9b, 0x6b, 0x00, 0x80, 0xc7, 0xbb, 0x59, 0x97) },
            { typeof(JTMappingPlaneAttributeElement),       new(0xa3cfb921, 0xbdeb, 0x48d7, 0xb3, 0x96, 0x8b, 0x8d, 0xe, 0xf4, 0x85, 0xa0) },
            { typeof(JTMappingCylinderAttributeElement),    new(0x3e70739d, 0x8cb0, 0x41ef, 0x84, 0x5c, 0xa1, 0x98, 0xd4, 0x0, 0x3b, 0x3f) },
            { typeof(JTMappingSphereAttributeElement),      new(0x72475fd1, 0x2823, 0x4219, 0xa0, 0x6c, 0xd9, 0xe6, 0xe3, 0x9a, 0x45, 0xc1) },
            { typeof(JTMappingTriplanarAttributeElement),   new(0x92f5b094, 0x6499, 0x4d2d, 0x92, 0xaa, 0x60, 0xd0, 0x5a, 0x44, 0x32, 0xcf) },
            { typeof(JTBasePropertyAtomElement),            new(0x10dd104b, 0x2ac8, 0x11d1, 0x9b, 0x6b, 0x00, 0x80, 0xc7, 0xbb, 0x59, 0x97) },
            { typeof(JTDatePropertyAtomElement),            new(0xce357246, 0x38fb, 0x11d1, 0xa5, 0x6, 0x0, 0x60, 0x97, 0xbd, 0xc6, 0xe1) },
            { typeof(JTIntegerPropertyAtomElement),         new(0x10dd102b, 0x2ac8, 0x11d1, 0x9b, 0x6b, 0x00, 0x80, 0xc7, 0xbb, 0x59, 0x97) },
            { typeof(JTFloatPropertyAtomElement),           new(0x10dd1019, 0x2ac8, 0x11d1, 0x9b, 0x6b, 0x00, 0x80, 0xc7, 0xbb, 0x59, 0x97) },
            { typeof(JTLateLoadedPropertyAtomElement),      new(0xe0b05be5, 0xfbbd, 0x11d1, 0xa3, 0xa7, 0x00, 0xaa, 0x00, 0xd1, 0x09, 0x54) },
            { typeof(JTObjectReferencePropertyAtomElement), new(0x10dd1004, 0x2ac8, 0x11d1, 0x9b, 0x6b, 0x00, 0x80, 0xc7, 0xbb, 0x59, 0x97) },
            { typeof(JTStringPropertyAtomElement),          new(0x10dd106e, 0x2ac8, 0x11d1, 0x9b, 0x6b, 0x00, 0x80, 0xc7, 0xbb, 0x59, 0x97) },
        };
        public static readonly Dictionary<GUID, Type> ObjectTypeIdentifiersReverse =
            ObjectTypeIdentifiers.ToDictionary((i) => i.Value, (i) => i.Key);
    }
}