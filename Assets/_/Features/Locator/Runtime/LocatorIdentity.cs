using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Locator.Runtime
{
    public class LocatorIdentity : MonoBehaviour
    {
        public Room m_currentRoom;
        public int m_priority;

        void OnEnable()
        {
            if (!LocatorSystem.m_locatorDict.ContainsKey(m_currentRoom))
            {
                LocatorSystem.m_locatorDict.Add(m_currentRoom, new List<LocatorIdentity>());
            }
            LocatorSystem.m_locatorDict[m_currentRoom].Add(this);
            LocatorSystem.m_locatorDict[m_currentRoom].Sort((p1, p2) => p1.m_priority.CompareTo(p2.m_priority));
        }

        void OnDisable()
        {
            LocatorSystem.m_locatorDict[m_currentRoom].Remove(this);
            LocatorSystem.m_locatorDict[m_currentRoom].Sort((p1, p2) => p1.m_priority.CompareTo(p2.m_priority));
        }
        private void OnValidate()
        {
            gameObject.name = $"[{m_priority}] [{m_currentRoom}] Locator";
        }
    }
}