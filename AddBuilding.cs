﻿using Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TUNING;

namespace WallPumps
{
    public static class AddBuilding
    {
        public static void AddBuildingToPlanScreen(HashedString category, string buildingID, string parentID = null)
        {
            int categoryIndex = GetCategoryIndex(category, buildingID);
            if (categoryIndex == -1) return;
            int? index = null;
            if (!parentID.IsNullOrWhiteSpace())
            {
                IList<string> list = BUILDINGS.PLANORDER[categoryIndex].data as IList<string>;
                if (list != null) index = list.IndexOf(parentID);
                if (index == -1)
                {
                    Console.WriteLine(string.Concat(new object[]
                    {
                    "ERROR: Building \"",
                    parentID,
                    "\" not found in category ",
                    category,
                    ". Placing ",
                    buildingID,
                    " at the end of the list"
                    }));
                } else
                {
                    index++; // Next index
                }
            }
            AddBuildingToPlanScreen(category, buildingID, index);
        }

        public static void AddBuildingToPlanScreen(HashedString category, string buildingID, int? index = null)
        {
            int categoryIndex = GetCategoryIndex(category, buildingID);
            if (categoryIndex == -1) return;
            IList<string> list = BUILDINGS.PLANORDER[categoryIndex].data as IList<string>;
            if (list == null) return;
            if (index != null && index.GetValueOrDefault() >= 0 && index.GetValueOrDefault() < list.Count)
            {
                list.Insert(index.GetValueOrDefault(), buildingID);
            } else
            {
                list.Add(buildingID);
            }
        }

        private static int GetCategoryIndex(HashedString category, string buildingID)
        {
            int num = BUILDINGS.PLANORDER.FindIndex(x => x.category == category);
            if (num == -1)
            {
                Console.WriteLine(string.Concat(new object[]
                {
                    "ERROR: Can't add building ",
                    buildingID,
                    " to non-existing category ",
                    category
                }));
            }
            return num;
        }

        public static void IntoTechTree(string tech, string buildingID)
        {
            List<string> list = new List<string>(Techs.TECH_GROUPING[tech]);
            list.Insert(1, buildingID);
            Techs.TECH_GROUPING[tech] = list.ToArray();
        }

        private static int GetTechCategoryIndex(HashedString category, string buildingID)
        {
            int num = BUILDINGS.PLANORDER.FindIndex((PlanScreen.PlanInfo x) => x.category == category);
            if (num == -1)
            {
                Console.WriteLine(string.Concat(new object[]
                {
                    "ERROR: Can't add building ",
                    buildingID,
                    " to non-existing category ",
                    category
                }));
            }
            return num;
        }

        public static void AddStrings(string ID, string name, string description, string effect)
        {
            Strings.Add(new string[]
            {
                "STRINGS.BUILDINGS.PREFABS." + ID.ToUpperInvariant() + ".NAME",
                string.Concat(new string[]
                {
                    "<link=\"",
                    ID,
                    "\">",
                    name,
                    "</link>"
                })
            });
            Strings.Add(new string[]
            {
                "STRINGS.BUILDINGS.PREFABS." + ID.ToUpperInvariant() + ".DESC",
                description
            });
            Strings.Add(new string[]
            {
                "STRINGS.BUILDINGS.PREFABS." + ID.ToUpperInvariant() + ".EFFECT",
                effect
            });
        }
    }
}
