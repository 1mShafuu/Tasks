#import <UnityFramework/UnityFramework-Swift.h>

extern "C"
{
    void SetKindredUserId(const char *userId)
    {
        [[KindredService shared] setUserIdWithUserId:[NSString stringWithUTF8String:userId]];
    }

    void SetKindredUserCountry(const char *userCountry)
    {
        [[KindredService shared] setUserCountryWithUserCountry:[NSString stringWithUTF8String:userCountry]];
    }

    void SetKindredAppScheme(const char *appScheme)
    {
        [[KindredService shared] setAppSchemeWithAppScheme:[NSString stringWithUTF8String:appScheme]];
    }

    void ShowKindredSettings()
    {
        [[KindredService shared] showSettings];
    }
}
