using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using NavMeshPlus.Extensions;

namespace NavMeshPlus.Components
{
    [ExecuteInEditMode]
    [DefaultExecutionOrder(-101)]
    [AddComponentMenu("Navigation/Navigation Link", 33)]
    [HelpURL("https://github.com/Unity-Technologies/NavMeshPlus#documentation-draft")]
    public class NavMeshLink : MonoBehaviour
    {
        [SerializeField, NavMeshAgent]
        int m_AgentTypeID;

        [System.Obsolete]

        public int agentTypeID { get { return m_AgentTypeID; } set { m_AgentTypeID = value; UpdateLink(); } }

        [SerializeField]
        Vector3 m_StartPoint = new Vector3(0.0f, 0.0f, -2.5f);

        [System.Obsolete]

        public Vector3 startPoint { get { return m_StartPoint; } set { m_StartPoint = value; UpdateLink(); } }

        [SerializeField]
        Vector3 m_EndPoint = new Vector3(0.0f, 0.0f, 2.5f);

        [System.Obsolete]

        public Vector3 endPoint { get { return m_EndPoint; } set { m_EndPoint = value; UpdateLink(); } }

        [SerializeField]
        float m_Width;

        [System.Obsolete]

        public float width { get { return m_Width; } set { m_Width = value; UpdateLink(); } }

        [SerializeField]
        int m_CostModifier = -1;

        [System.Obsolete]

        public int costModifier { get { return m_CostModifier; } set { m_CostModifier = value; UpdateLink(); } }

        [SerializeField]
        bool m_Bidirectional = true;

        [System.Obsolete]

        public bool bidirectional { get { return m_Bidirectional; } set { m_Bidirectional = value; UpdateLink(); } }

        [SerializeField]
        bool m_AutoUpdatePosition;

        [System.Obsolete]

        public bool autoUpdate { get { return m_AutoUpdatePosition; } set { SetAutoUpdate(value); } }

        [SerializeField, NavMeshArea]
        int m_Area;

        [System.Obsolete]

        public int area { get { return m_Area; } set { m_Area = value; UpdateLink(); } }

        NavMeshLinkInstance m_LinkInstance = new NavMeshLinkInstance();

        Vector3 m_LastPosition = Vector3.zero;
        Quaternion m_LastRotation = Quaternion.identity;

        static readonly List<NavMeshLink> s_Tracked = new List<NavMeshLink>();

        [System.Obsolete]
        void OnEnable()
        {
            AddLink();
            if (m_AutoUpdatePosition && m_LinkInstance.valid)
                AddTracking(this);
        }

        [System.Obsolete]
        void OnDisable()
        {
            RemoveTracking(this);
            m_LinkInstance.Remove();
        }

        [System.Obsolete]
        public void UpdateLink()
        {
            m_LinkInstance.Remove();
            AddLink();
        }

        [System.Obsolete]
        static void AddTracking(NavMeshLink link)
        {
#if UNITY_EDITOR
            if (s_Tracked.Contains(link))
            {
                Debug.LogError("Link is already tracked: " + link);
                return;
            }
#endif

            if (s_Tracked.Count == 0)
                NavMesh.onPreUpdate += UpdateTrackedInstances;

            s_Tracked.Add(link);
        }

        [System.Obsolete]
        static void RemoveTracking(NavMeshLink link)
        {
            s_Tracked.Remove(link);

            if (s_Tracked.Count == 0)
                NavMesh.onPreUpdate -= UpdateTrackedInstances;
        }

        [System.Obsolete]
        void SetAutoUpdate(bool value)
        {
            if (m_AutoUpdatePosition == value)
                return;
            m_AutoUpdatePosition = value;
            if (value)
                AddTracking(this);
            else
                RemoveTracking(this);
        }

        [System.Obsolete]
        void AddLink()
        {
#if UNITY_EDITOR
            if (m_LinkInstance.valid)
            {
                Debug.LogError("Link is already added: " + this);
                return;
            }
#endif

            var link = new NavMeshLinkData();
            link.startPosition = m_StartPoint;
            link.endPosition = m_EndPoint;
            link.width = m_Width;
            link.costModifier = m_CostModifier;
            link.bidirectional = m_Bidirectional;
            link.area = m_Area;
            link.agentTypeID = m_AgentTypeID;
            m_LinkInstance = NavMesh.AddLink(link, transform.position, transform.rotation);
            if (m_LinkInstance.valid)
                m_LinkInstance.owner = this;

            m_LastPosition = transform.position;
            m_LastRotation = transform.rotation;
        }

        bool HasTransformChanged()
        {
            if (m_LastPosition != transform.position) return true;
            if (m_LastRotation != transform.rotation) return true;
            return false;
        }

        [System.Obsolete]
        void OnDidApplyAnimationProperties()
        {
            UpdateLink();
        }

        [System.Obsolete]
        static void UpdateTrackedInstances()
        {
            foreach (var instance in s_Tracked)
            {
                if (instance.HasTransformChanged())
                    instance.UpdateLink();
            }
        }

#if UNITY_EDITOR
        [System.Obsolete]

        void OnValidate()
        {
            m_Width = Mathf.Max(0.0f, m_Width);

            if (!m_LinkInstance.valid)
                return;

            UpdateLink();

            if (!m_AutoUpdatePosition)
            {
                RemoveTracking(this);
            }
            else if (!s_Tracked.Contains(this))
            {
                AddTracking(this);
            }
        }
#endif
    }
}
