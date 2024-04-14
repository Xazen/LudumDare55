/////////////////////////////////////////////////////////////////////////////////////////////////////
//
// Audiokinetic Wwise generated include file. Do not edit.
//
/////////////////////////////////////////////////////////////////////////////////////////////////////

#ifndef __WWISE_IDS_H__
#define __WWISE_IDS_H__

#include <AK/SoundEngine/Common/AkTypes.h>

namespace AK
{
    namespace EVENTS
    {
        static const AkUniqueID PAUSE = 3092587493U;
        static const AkUniqueID RESUME = 953277036U;
        static const AkUniqueID STARTGAMEMUSIC = 1568977742U;
        static const AkUniqueID STOPGAMEMUSIC = 2221504388U;
        static const AkUniqueID STOPGAMEMUSIC_IMMEDIATE = 3345735074U;
    } // namespace EVENTS

    namespace STATES
    {
        namespace DIFFICULTY
        {
            static const AkUniqueID GROUP = 290942698U;

            namespace STATE
            {
                static const AkUniqueID EASY = 529018163U;
                static const AkUniqueID HARD = 3599861390U;
                static const AkUniqueID MEDIUM = 2849147824U;
                static const AkUniqueID NONE = 748895195U;
            } // namespace STATE
        } // namespace DIFFICULTY

        namespace GAMESTATE
        {
            static const AkUniqueID GROUP = 4091656514U;

            namespace STATE
            {
                static const AkUniqueID LOST = 221232711U;
                static const AkUniqueID MAINMENU = 3604647259U;
                static const AkUniqueID NONE = 748895195U;
                static const AkUniqueID STARTGAME = 1521187885U;
                static const AkUniqueID WON = 1080430619U;
            } // namespace STATE
        } // namespace GAMESTATE

    } // namespace STATES

    namespace BANKS
    {
        static const AkUniqueID INIT = 1355168291U;
        static const AkUniqueID GENERAL = 133642231U;
    } // namespace BANKS

    namespace BUSSES
    {
        static const AkUniqueID MASTER_AUDIO_BUS = 3803692087U;
    } // namespace BUSSES

    namespace AUDIO_DEVICES
    {
        static const AkUniqueID NO_OUTPUT = 2317455096U;
        static const AkUniqueID SYSTEM = 3859886410U;
    } // namespace AUDIO_DEVICES

}// namespace AK

#endif // __WWISE_IDS_H__
