namespace OpenMEPSandbox.Autocad
{
    public class Database
    {
        private Database()
        {
        
        }

        /// <summary>
        /// Get User Coordinate Systems Of Autocad Database
        /// </summary>
        /// <param name="AcadDatabase"></param>
        /// <returns name="AcadUCSs">AcadUCSs</returns>
        public static dynamic UserCoordinateSystems(dynamic AcadDatabase)
        {
            List<dynamic> ucss = new List<dynamic>();
            int count = AcadDatabase.UserCoordinateSystems.Count;
            for (int i = 0; i < count; i++)
            {
                dynamic ucs = AcadDatabase.UserCoordinateSystems.Item(i);
                ucss.Add(ucs);
            }
            return ucss;
        }

    }
}