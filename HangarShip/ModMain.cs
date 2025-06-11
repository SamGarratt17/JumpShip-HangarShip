using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using MelonLoader;
using UnityEngine.SceneManagement;
using System.Collections;

namespace HangarShip
{
    public class ModMain : MelonMod
    {
        public override void OnInitializeMelon()
        {
            LoggerInstance.Msg("HangarShip Melon Enabled");
        }

        public override void OnSceneWasInitialized(int buildIndex, string sceneName)
        {
          if (sceneName == "Dest_Lobby_Start_Art")
            {
                MelonCoroutines.Start(DelayedActivate());
                LoggerInstance.Msg($"Scene {sceneName} with build index {buildIndex} has been loaded!");
            }
        }

        private IEnumerator DelayedActivate()
        {
            yield return new WaitForSeconds(3f);
            EnableShip.Activate();
        }
    }

    public static class EnableShip
    {
        public static void Activate()
        {
            try
            {
                GameObject parent1 = GameObject.Find("PF_LocalSpace");
                if (parent1 != null)
                {
                    Transform playerShip = parent1.transform.Find("PlayerShip");
                    if (playerShip != null)
                    {
                        playerShip.gameObject.SetActive(true);

                        Transform grapplePoints = playerShip.Find("PF_SpaceShip/ExteriorGameplay/GrapplePoints");
                        if (grapplePoints != null)
                        {
                            for (int i = 1; i <= 14; i++)
                            {
                                string floorName = $"PF_GrapplePoint_Floor_{i:00}";
                                Transform floorPoint = grapplePoints.Find(floorName);
                                if (floorPoint != null)
                                {
                                    floorPoint.gameObject.SetActive(true);
                                }
                            }

                            Transform edgePoint = grapplePoints.Find("PF_GrapplePoint_Edge");
                            if (edgePoint != null)
                            {
                                edgePoint.gameObject.SetActive(true);
                            }
                        }
                    }
                }

                GameObject gameplayParent = GameObject.Find("Gameplay");
                if (gameplayParent != null)
                {
                    Transform dockingnode = gameplayParent.transform.Find("PF_Dockingnode");
                    if (dockingnode == null) MelonLogger.Error("docking not found");

                    Transform shipFly = dockingnode.Find("ShipFlyDirector");
                    if (shipFly == null) MelonLogger.Error("shipFly not found");

                    Transform parent = shipFly.Find("CinematicShipParent");
                    if (parent == null) MelonLogger.Error("parent not found");

                    Transform parentTransform = parent.Find("CinematicShipParent_TRANSFORM");
                    if (parentTransform == null) MelonLogger.Error("transform not found");

                    Transform cinematicRoot = parentTransform.Find("Cinematic Ship");
                    if (cinematicRoot == null) MelonLogger.Error("root not found");

                    Transform exterior = cinematicRoot.Find("Exterior");
                    if (exterior != null) exterior.gameObject.SetActive(false);

                    Transform exteriorGameplay = cinematicRoot.Find("ExteriorGameplay");
                    if (exteriorGameplay != null) exteriorGameplay.gameObject.SetActive(false);

                    Transform transitionGameplay = cinematicRoot.Find("TransitionGameplay");
                    if (transitionGameplay != null) transitionGameplay.gameObject.SetActive(false);

                    Transform interiorNoCulling = cinematicRoot.Find("Interior_NoCulling");
                    if (interiorNoCulling != null) interiorNoCulling.gameObject.SetActive(false);

                    Transform interior = cinematicRoot.Find("Interior");
                    if (interior != null) interior.gameObject.SetActive(false);

                    Transform interiorGameplay = cinematicRoot.Find("InteriorGameplay");
                    if (interiorGameplay != null) interiorGameplay.gameObject.SetActive(false);

                    Transform killTrigger = gameplayParent.transform.Find("PF_KillTrigger");
                    if (killTrigger != null)
                    {
                        killTrigger.gameObject.SetActive(false);
                    }
                }
            }
            catch { }
        }

        private static Transform FindDeepChild(Transform parent, string name)
        {
            for (int i = 0; i < parent.childCount; i++)
            {
                Transform child = parent.GetChild(i);
                if (child.name == name)
                    return child;
                Transform result = FindDeepChild(child, name);
                if (result != null)
                    return result;
            }
            return null;
        }
    }


}