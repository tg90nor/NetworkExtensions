﻿using System;
using System.Collections.Generic;
using System.Linq;
using Transit.Addon.RoadExtensions.Menus;
using Transit.Addon.RoadExtensions.Props;
using Transit.Framework;
using Transit.Framework.Builders;
using UnityEngine;
using Debug = Transit.Framework.Debug;

namespace Transit.Addon.RoadExtensions.Roads.Busway2L
{
    public class Busway2LBuilder : Activable, IMultiNetInfoBuilderPart
    {
        public string Name { get { return "Small Busway"; } }
        public string DisplayName { get { return "Busway"; } }
        public string BasedPrefabName { get { return NetInfos.Vanilla.ROAD_2L; } }
        public int Order { get { return 110; } }
        public NetInfoVersion SupportedVersions { get { return NetInfoVersion.AllWithDecoration; } }

        public IEnumerable<IMenuItemBuilder> MenuItemBuilders
        {
            get
            {
                yield return new MenuItemBuilder
                {
                    UICategory = AdditionnalMenus.ROADS_BUSWAYS,
                    UIOrder = 10,
                    Name = "Small Busway",
                    DisplayName = "Busway",
                    Description = "A two-lane, two-way road suitable for buses only. Busway does not allow zoning next to it!",
                    ThumbnailsPath = @"Roads\Busway2L\thumbnails.png",
                    InfoTooltipPath = @"Roads\Busway2L\infotooltip.png"
                };
                yield return new MenuItemBuilder
                {
                    UICategory = AdditionnalMenus.ROADS_BUSWAYS,
                    UIOrder = 11,
                    Name = "Small Busway Decoration Grass",
                    DisplayName = "Busway with Grass",
                    Description = "A two-lane, two-way road with decorative grass suitable for buses only. Busway does not allow zoning next to it!",
                    ThumbnailsPath = @"Roads\Busway2L\thumbnails_grass.png",
                    InfoTooltipPath = @"Roads\Busway2L\infotooltip_grass.png"
                };
                yield return new MenuItemBuilder
                {
                    UICategory = AdditionnalMenus.ROADS_BUSWAYS,
                    UIOrder = 12,
                    Name = "Small Busway Decoration Trees",
                    DisplayName = "Busway with Trees",
                    Description = "A two-lane, two-way road with decorative trees suitable for buses only. Busway does not allow zoning next to it!",
                    ThumbnailsPath = @"Roads\Busway2L\thumbnails_trees.png",
                    InfoTooltipPath = @"Roads\Busway2L\infotooltip_trees.png"
                };
            }
        }

        public void BuildUp(NetInfo info, NetInfoVersion version)
        {
            ///////////////////////////
            // Texturing             //
            ///////////////////////////
            switch (version)
            {
                case NetInfoVersion.Ground:
                    {
                        foreach (var segment in info.m_segments)
                        {
                            switch (segment.m_forwardRequired)
                            {
                                case NetSegment.Flags.StopLeft:
                                case NetSegment.Flags.StopRight:
                                    segment.SetTextures(
                                        new TexturesSet
                                            (@"Roads\Busway2L\Textures\Ground_Segment__MainTex.png",
                                             @"Roads\Busway2L\Textures\Ground_Segment__AlphaMap.png"),
                                        new TexturesSet
                                            (@"Roads\Busway2L\Textures\Ground_SegmentLOD_Bus__MainTex.png",
                                             @"Roads\Busway2L\Textures\Ground_SegmentLOD_Bus__AlphaMap.png",
                                             @"Roads\Busway2L\Textures\Ground_SegmentLOD__XYSMap.png"));
                                    break;

                                case NetSegment.Flags.StopBoth:
                                    segment.SetTextures(
                                        new TexturesSet
                                            (@"Roads\Busway2L\Textures\Ground_Segment__MainTex.png",
                                             @"Roads\Busway2L\Textures\Ground_Segment__AlphaMap.png"),
                                        new TexturesSet
                                            (@"Roads\Busway2L\Textures\Ground_SegmentLOD_BusBoth__MainTex.png",
                                             @"Roads\Busway2L\Textures\Ground_SegmentLOD_BusBoth__AlphaMap.png",
                                             @"Roads\Busway2L\Textures\Ground_SegmentLOD__XYSMap.png"));
                                    break;

                                default:
                                    segment.SetTextures(
                                        new TexturesSet
                                            (@"Roads\Busway2L\Textures\Ground_Segment__MainTex.png",
                                             @"Roads\Busway2L\Textures\Ground_Segment__AlphaMap.png"),
                                        new TexturesSet
                                            (@"Roads\Busway2L\Textures\Ground_SegmentLOD__MainTex.png",
                                             @"Roads\Busway2L\Textures\Ground_SegmentLOD__AlphaMap.png",
                                             @"Roads\Busway2L\Textures\Ground_SegmentLOD__XYSMap.png"));
                                    break;
                            }
                        }
                    }
                    break;

                case NetInfoVersion.GroundGrass:
                case NetInfoVersion.GroundTrees:
                    {
                        foreach (var segment in info.m_segments)
                        {
                            switch (segment.m_forwardRequired)
                            {
                                case NetSegment.Flags.StopLeft:
                                case NetSegment.Flags.StopRight:
                                    segment.SetTextures(
                                        new TexturesSet
                                            (@"Roads\Busway2L\Textures_Grass\Ground_Segment__MainTex.png",
                                             @"Roads\Busway2L\Textures_Grass\Ground_Segment_Bus__AlphaMap.png"));
                                    break;

                                case NetSegment.Flags.StopBoth:
                                    segment.SetTextures(
                                        new TexturesSet
                                            (@"Roads\Busway2L\Textures_Grass\Ground_Segment__MainTex.png",
                                             @"Roads\Busway2L\Textures_Grass\Ground_Segment_BusBoth__AlphaMap.png"));
                                    break;

                                default:
                                    segment.SetTextures(
                                        new TexturesSet
                                            (@"Roads\Busway2L\Textures_Grass\Ground_Segment__MainTex.png",
                                             @"Roads\Busway2L\Textures_Grass\Ground_Segment__AlphaMap.png"));
                                    break;
                            }
                        }
                    }
                    break;

                case NetInfoVersion.Bridge:
                case NetInfoVersion.Elevated:
                    {
                        foreach (var segment in info.m_segments)
                        {
                            segment.SetTextures(
                                new TexturesSet
                                    (@"Roads\Busway2L\Textures\Elevated_Segment__MainTex.png",
                                     @"Roads\Busway2L\Textures\Elevated_Segment__AlphaMap.png"),
                                new TexturesSet
                                    (@"Roads\Busway2L\Textures\Elevated_SegmentLOD__MainTex.png",
                                     @"Roads\Busway2L\Textures\Elevated_SegmentLOD__AlphaMap.png",
                                     @"Roads\Busway2L\Textures\Elevated_SegmentLOD__XYSMap.png"));
                        }
                    }
                    break;

                case NetInfoVersion.Slope:
                    {
                        foreach (var segment in info.m_segments)
                        {
                            segment.SetTextures(
                                new TexturesSet
                                    (@"Roads\Busway2L\Textures\Slope_Segment__MainTex.png",
                                     @"Roads\Busway2L\Textures\Slope_Segment__AlphaMap.png"),
                                new TexturesSet
                                    (@"Roads\Busway2L\Textures\Slope_SegmentLOD__MainTex.png",
                                     @"Roads\Busway2L\Textures\Slope_SegmentLOD__AlphaMap.png",
                                     @"Roads\Busway2L\Textures\Slope_SegmentLOD__XYS.png"));
                        }
                    }
                    break;
                case NetInfoVersion.Tunnel:
                    break;
            }

            ///////////////////////////
            // Templates             //
            ///////////////////////////
            var highwayInfo = Prefabs.Find<NetInfo>(NetInfos.Vanilla.HIGHWAY_3L);

            ///////////////////////////
            // Set up                //
            ///////////////////////////
            info.m_UnlockMilestone = highwayInfo.m_UnlockMilestone;

            var prop = Prefabs.Find<PropInfo>("BusLaneText", false);

            if (prop != null)
            {
                foreach (var lane in info.m_lanes.Where(l => l.m_laneType == NetInfo.LaneType.Vehicle))
                {
                    if (lane.m_laneProps == null)
                    {
                        lane.m_laneProps = ScriptableObject.CreateInstance<NetLaneProps>();
                        lane.m_laneProps.m_props = new NetLaneProps.Prop[] { };
                    }
                    else
                    {
                        lane.m_laneProps = lane.m_laneProps.Clone();
                    }

                    lane.m_laneProps.m_props = lane
                        .m_laneProps
                        .m_props
                            .Union(new NetLaneProps.Prop
                            {
                                m_prop = prop,
                                m_position = new Vector3(0f, 0f, 7.5f),
                                m_angle = 180f,
                                m_segmentOffset = -1f,
                                m_minLength = 8f,
                                m_startFlagsRequired = NetNode.Flags.Junction
                            })
                        .ToArray();
                }
            }

            foreach (var lane in info.m_lanes)
            {
                if (lane.m_laneType == NetInfo.LaneType.Vehicle)
                {
                    lane.m_speedLimit = 1.6f;
                    lane.m_laneType = NetInfo.LaneType.TransportVehicle;
                }
            }

            var roadBaseAI = info.GetComponent<RoadBaseAI>();

            if (roadBaseAI != null)
            {
            }

            var roadAI = info.GetComponent<RoadAI>();

            if (roadAI != null)
            {
                roadAI.m_enableZoning = false;
            }
        }
    }
}
