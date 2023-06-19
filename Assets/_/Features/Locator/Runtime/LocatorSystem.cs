using System.Collections.Generic;
using UnityEngine;

namespace Locator.Runtime
{
    public enum Room
    {
        Land,
        Mine,
        Bell,
    }
    
    public class LocatorSystem
    {

        #region Public Memebers

        public static Dictionary<Room, List<LocatorIdentity>> m_locatorDict = new Dictionary<Room, List<LocatorIdentity>>();

        #endregion


        #region Main Methods

        public static LocatorIdentity GetFirstLocationPriority(Room currentRoom)
        {
            LocatorIdentity currentLocation = m_locatorDict[currentRoom][0];

            for (int i = 1; i < m_locatorDict[currentRoom].Count; i++)
            {
                if (m_locatorDict[currentRoom][i].m_priority < currentLocation.m_priority)
                {
                    currentLocation = m_locatorDict[currentRoom][i];
                }
            }
            return currentLocation;
        }

        public static LocatorIdentity GetNextLocation(Room currentRoom, LocatorIdentity currentLocation)
        {
            LocatorIdentity locationResult = m_locatorDict[currentRoom][0];

            if (currentLocation == m_locatorDict[currentRoom][m_locatorDict[currentRoom].Count - 1])
            {
                return locationResult;
            }

            for (int i = 0; i < m_locatorDict[currentRoom].Count; i++)
            {
                if (m_locatorDict[currentRoom][i] == currentLocation)
                {
                    locationResult = m_locatorDict[currentRoom][i + 1];
                }
            }
            return locationResult;
        }

        public static LocatorIdentity GetPreviousLocation(Room currentRoom, LocatorIdentity currentLocation)
        {
            LocatorIdentity locationResult = m_locatorDict[currentRoom][m_locatorDict[currentRoom].Count - 1];

            if (currentLocation == m_locatorDict[currentRoom][0])
            {
                return locationResult;
            }

            for (int i = 0; i < m_locatorDict[currentRoom].Count; i++)
            {
                if (m_locatorDict[currentRoom][i] == currentLocation)
                {
                    locationResult = m_locatorDict[currentRoom][i - 1];
                }
            }
            return locationResult;
        }

        public static LocatorIdentity GetNearestLocation(Room currentRoom, Vector3 pos, bool stayInSameRoom = true)
        {
            LocatorIdentity nearestLocation = m_locatorDict[currentRoom][0];

            for (int i = 1; i < m_locatorDict[currentRoom].Count; i++)
            {
                LocatorIdentity checkPos = m_locatorDict[currentRoom][i];
                if (Vector3.Distance(pos, checkPos.transform.position) < Vector3.Distance(pos, nearestLocation.transform.position))
                {
                    nearestLocation = checkPos;
                }
            }
            return nearestLocation;
        }

        public static LocatorIdentity GetRandomLocation(Room currentRoom, bool stayInSameRoom = true)
        {
            return m_locatorDict[currentRoom][Random.Range(0, m_locatorDict[currentRoom].Count)];
        }

        #endregion
    }
}