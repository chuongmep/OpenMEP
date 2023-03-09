Autodesk.Internal.InfoCenter.InfoToolbar autodeskInformaton =ComponentManager.InfoCenterToolBar;
        for(int i = 0; i < autodeskInformaton.Items.Count; i++)
        {
            if(autodeskInformaton.Items[i] is Autodesk.Internal.InfoCenter.WebServicesLoginButton)
            {
                string autodeskID = ((autodeskInformaton.Items[i] as Autodesk.Internal.InfoCenter.WebServicesLoginButton)!).Text;
                Trace.WriteLine(autodeskID);
            }
        }