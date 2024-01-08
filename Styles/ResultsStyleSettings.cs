namespace IcukBroadbandCheckerCs.Styles {
    /**
     * Contains values that can be customised to change the style of the results module of the broadband 
     * availability checker.
     */
    public class ResultsStyleSettings {
        // Background Colors
        public string BackgroundColour = "#fff";
        public string HeadBackgroundColour = "#255B76";
        public string LeftBackgroundColour = "#626262";
        public string SeperatorsColour = "#ccc";

        // Text Colors
        public string TextColour = "#000";
        public string HeadTextColour = "#fff";
        public string LeftTextColour = "#fff";
        public string AvailableTextColour = "#468847";
        public string NotAvailableTextColour = "#c00";

        // Label Colors
        public string AvailableLabelColour = "#fff";
        public string NotAvailableLabelColour = "#fff";

        // Loading Colors
        public string LoadingCirclePrimaryColour = "#3498db";
        public string LoadingCircleSecondaryColour = "#f3f3f3";

        // Miscellaneous
        public bool HideResults = true;
    }
}