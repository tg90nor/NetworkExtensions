﻿using System.Linq;
using System.Collections.Generic;

using Transit.Addon.RoadExtensions.Menus;
using Transit.Addon.RoadExtensions.Menus.Roads;
using Transit.Addon.RoadExtensions.Roads.Common;
using Transit.Framework;
using Transit.Framework.Builders;
using Transit.Framework.Network;
namespace Transit.Addon.RoadExtensions.Roads.SmallHeavyRoads.OneWay3LTramAndBus
{
    public partial class OneWay3LTramAndBusBuilder : Activable, INetInfoBuilderPart
    {
        public int Order { get { return 21; } }
        public int UIOrder { get { return 21; } }

        public string BasedPrefabName { get { return NetInfos.Vanilla.ONEWAY_2L; } }
        public string Name { get { return "OneWay3LTramAndBus"; } }
        public string DisplayName { get { return "Three-Lane Oneway with Bus-Tram"; } }
        public string Description { get { return "A three-lane one-way road with bus-bicycle spaces. Supports medium traffic."; } }
        public string ShortDescription { get { return "Zoneable, medium traffic"; } }
        public string UICategory { get { return RExExtendedMenus.ROADS_SMALL_HV; } }

        public string ThumbnailsPath    { get { return @"Roads\SmallHeavyRoads\OneWay3LBikeAndBus\thumbnails.png"; } }
        public string InfoTooltipPath   { get { return @"Roads\SmallHeavyRoads\OneWay3LBikeAndBus\infotooltip.png"; } }

        public NetInfoVersion SupportedVersions
        {
            get { return NetInfoVersion.All; }
        }

        public void BuildUp(NetInfo info, NetInfoVersion version)
        {
            ///////////////////////////
            // Template              //
            ///////////////////////////
            var owRoadInfo = Prefabs.Find<NetInfo>(NetInfos.Vanilla.ONEWAY_2L);
            var owRoadTunnelInfo = Prefabs.Find<NetInfo>(NetInfos.Vanilla.ONEWAY_2L_TUNNEL);

            ///////////////////////////
            // 3DModeling            //
            ///////////////////////////
            info.Setup16m2mSWMeshTram(version, LanesLayoutStyle.AsymL1R2);

            ///////////////////////////
            // Texturing             //
            ///////////////////////////
            SetupTextures(info, version);

            ///////////////////////////
            // Set up                //
            ///////////////////////////
            info.m_hasParkingSpaces = false;
          
            info.m_pavementWidth = (version != NetInfoVersion.Slope && version != NetInfoVersion.Tunnel ? 2 : 5);
            info.m_halfWidth = (version != NetInfoVersion.Slope && version != NetInfoVersion.Tunnel ? 8 : 11);
          
            if (version == NetInfoVersion.Tunnel)
            {
                info.m_setVehicleFlags = Vehicle.Flags.Transition | Vehicle.Flags.Underground;
                info.m_setCitizenFlags = CitizenInstance.Flags.Transition | CitizenInstance.Flags.Underground;
                info.m_class = owRoadTunnelInfo.m_class.Clone(NetInfoClasses.NEXT_SMALL3L_ROAD_TUNNEL);
            }
            else
            {
                info.m_class = owRoadInfo.m_class.Clone(NetInfoClasses.NEXT_SMALL4L_ROAD);
            }
            var LaneWidth = 3.0f;
            // Setting up lanes
            info.SetRoadLanes(version, new LanesConfiguration
            {
                IsTwoWay = false,
                LanesToAdd = 3,
                PedPropOffsetX = 0.5f,
                BusStopOffset = 0f,
                SpeedLimit = 1.2f,
                LaneWidth = LaneWidth,
              
                LayoutStyle = LanesLayoutStyle.AsymL1R2
            });


            var carLanes = info.m_lanes.Where(l => l.m_laneType == NetInfo.LaneType.Vehicle).ToList();
            var pedkLanes = info.m_lanes.Where(l => l.m_laneType == NetInfo.LaneType.Pedestrian).ToList();

            for (var i = 0; i < carLanes.Count; i++)
            {
                if (i == 2)
                {
                    //RoadHelper.SetupTurningLaneProps(carLanes[i]);
                }
                if (i == 3)
                {
                    carLanes[i].m_vehicleType = VehicleInfo.VehicleType.Car;
                    carLanes[i].m_laneType = NetInfo.LaneType.TransportVehicle;
                 //   carLanes[i].SetSharedLaneProps();
                    var tempProps = carLanes[i].m_laneProps.m_props.ToList();
                    tempProps.RemoveProps("arrow");
                    carLanes[i].m_laneProps.m_props = tempProps.ToArray();
                }
                if (i == 4)
                {
                    carLanes[i].m_position = carLanes[3].m_position;
                    carLanes[i].m_vehicleType = VehicleInfo.VehicleType.Tram;
                    carLanes[i].m_speedLimit = .6f;
                    var tempProps = carLanes[i].m_laneProps.m_props.ToList();
                    tempProps.RemoveProps("arrow");
                    carLanes[i].m_laneProps.m_props = tempProps.ToArray();
                }

            }
            var tempLanes = new List<NetInfo.Lane>();
            tempLanes.AddRange(pedkLanes);
            tempLanes.AddRange(carLanes);
            info.m_lanes = tempLanes.ToArray();

            var leftPedLane = info.GetLeftRoadShoulder();
            var rightPedLane = info.GetRightRoadShoulder();

            //Setting Up Props
            var leftRoadProps = leftPedLane.m_laneProps.m_props.ToList();
            var rightRoadProps = rightPedLane.m_laneProps.m_props.ToList();

            if (version == NetInfoVersion.Slope)
            {
                leftRoadProps.AddLeftWallLights(info.m_pavementWidth);
                rightRoadProps.AddRightWallLights(info.m_pavementWidth);
            }

            leftPedLane.m_laneProps.m_props = leftRoadProps.ToArray();
            rightPedLane.m_laneProps.m_props = rightRoadProps.ToArray();

            info.TrimAboveGroundProps(version);
            info.SetupNewSpeedLimitProps(60, 40);


            // AI
            var owPlayerNetAI = owRoadInfo.GetComponent<PlayerNetAI>();
            var playerNetAI = info.GetComponent<PlayerNetAI>();

            if (owPlayerNetAI != null && playerNetAI != null)
            {
                playerNetAI.m_constructionCost = owPlayerNetAI.m_constructionCost * 2; // Charge by the lane?
                playerNetAI.m_maintenanceCost = owPlayerNetAI.m_maintenanceCost * 2; // Charge by the lane?
            }

            var roadBaseAI = info.GetComponent<RoadBaseAI>();

            if (roadBaseAI != null)
            {
                roadBaseAI.m_trafficLights = true;
            }
        }
    }
}