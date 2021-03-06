using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace uAdventure.Runner
{
    public class uAdventureRaycaster : PhysicsRaycaster
    {
        private RaycastHit[] m_Hits;
        private RaycastHit2D[] m_Hits2D;

        public bool Disabled { get; set; }
        public GameObject Override { get; set; }
        public GameObject Base { get; set; }

        public static uAdventureRaycaster Instance 
        { 
            get
            {
                return FindObjectOfType<uAdventureRaycaster>();
            }
        }

        protected override void Awake()
        {
            base.Awake();
            Disabled = false;
        }

        public override void Raycast(PointerEventData eventData, List<RaycastResult> resultAppendList)
        {
            if (Disabled)
                return;

            if (Override != null)
            {
                var result = new RaycastResult
                {
                    gameObject = Override,
                    module = this,
                    screenPosition = eventData.position,
                    index = resultAppendList.Count,
                    sortingLayer = 0,
                    sortingOrder = 1,
                    distance = int.MaxValue,
                    depth = int.MaxValue
                };
                resultAppendList.Add(result);
            }

            if (eventCamera == null)
                return;

            Ray ray;
            float distanceToClipPlane;
            ComputeRayAndDistance(eventData, out ray, out distanceToClipPlane);

            int hitCount = 0, hitCount2D = 0;

            if (m_MaxRayIntersections == 0)
            {
                m_Hits = Physics.RaycastAll(ray, distanceToClipPlane, finalEventMask);
                hitCount = m_Hits.Length;
            }
            else
            {
                if (m_LastMaxRayIntersections != m_MaxRayIntersections)
                {
                    m_Hits = new RaycastHit[m_MaxRayIntersections];
                    m_LastMaxRayIntersections = m_MaxRayIntersections;
                }

                hitCount = Physics.RaycastNonAlloc(ray, m_Hits, distanceToClipPlane, finalEventMask);
            }

            m_Hits2D = Physics2D.GetRayIntersectionAll(ray, distanceToClipPlane, finalEventMask);
            hitCount2D = m_Hits2D.Length;

            if (hitCount > 1)
            {
                System.Array.Sort(m_Hits, (r1, r2) => r1.distance.CompareTo(r2.distance));
            }

            if (hitCount != 0)
            {
                for (int b = 0, bmax = hitCount; b < bmax; ++b)
                {
                    var hitTransparent = m_Hits[b].collider.gameObject.GetComponent<Transparent>();

                    if (hitTransparent && hitTransparent.CheckTransparency(m_Hits[b]))
                    {
                        continue;
                    }
    
                    var result = new RaycastResult
                    {
                        gameObject = m_Hits[b].collider.gameObject,
                        module = this,
                        distance = m_Hits[b].distance,
                        worldPosition = m_Hits[b].point,
                        worldNormal = m_Hits[b].normal,
                        screenPosition = eventData.position,
                        index = resultAppendList.Count,
                        sortingLayer = 0,
                        sortingOrder = 0
                    };
                    resultAppendList.Add(result);
                }
            }

            if (hitCount2D != 0)
            {
                for (int b = 0, bmax = hitCount2D; b < bmax; ++b)
                {
                    var sr = m_Hits2D[b].collider.gameObject.GetComponent<SpriteRenderer>();
                    var hitPosition = new Vector3(m_Hits2D[b].point.x, m_Hits2D[b].point.y, m_Hits2D[b].collider.gameObject.transform.position.z);

                    var result = new RaycastResult
                    {
                        gameObject = m_Hits2D[b].collider.gameObject,
                        module = this,
                        distance = Vector3.Distance(eventCamera.transform.position, hitPosition),
                        worldPosition = hitPosition,
                        worldNormal = m_Hits2D[b].normal,
                        screenPosition = eventData.position,
                        index = resultAppendList.Count,
                        sortingLayer = sr != null ? sr.sortingLayerID : 0,
                        sortingOrder = sr != null ? sr.sortingOrder : 0
                    };
                    resultAppendList.Add(result);
                }
            }

            if (Base != null)
            {
                var result = new RaycastResult
                {
                    gameObject = Base,
                    module = this,
                    screenPosition = eventData.position,
                    index = -1,
                    sortingLayer = 0,
                    sortingOrder = 0,
                    distance = int.MinValue,
                    depth = int.MinValue
                };
                resultAppendList.Add(result);
            }
        }

    }
}