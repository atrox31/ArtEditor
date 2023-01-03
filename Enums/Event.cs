namespace ArtCore_Editor.Enums
{
    public static class Event
    {
        public enum EventType
        {
            EvOnCreate,
            EvOnDestroy,

            EvOnKeyDown,
            EvOnKeyUp,

            EvOnMouseMotion,
            EvOnMouseWheel,
            EvOnMouseDown,
            EvOnMouseUp,

            EvOnCollision,

            EvOnViewEnter,
            EvOnViewLeave,

            EvClicked,
            EvTrigger,

            EvStep,
            EvDraw,

        }
        public enum TriggerType
        {
            EvOnClick,
            EvOnHover
        }
    }
}
