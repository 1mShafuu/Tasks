import Foundation

@objc public class KindredService: NSObject
{
    @objc public static let shared = KindredService()

    var defaults = UserDefaults()
    let userIdKey = "KKUserId"
    let userCountryKey = "KKUserCountry"
    let appSchemeKey = "KKAppScheme"

    @objc public func setUserId(userId:String) -> Void {
        if let groupName = Bundle.main.object(forInfoDictionaryKey: "AppGroupName") as? String {
            defaults = UserDefaults.init(suiteName: groupName)!
            defaults.setValue(userId, forKey: userIdKey)
        }
    }
    
    @objc public func setUserCountry(userCountry:String) -> Void {
        if let groupName = Bundle.main.object(forInfoDictionaryKey: "AppGroupName") as? String {
            defaults = UserDefaults.init(suiteName: groupName)!
            defaults.setValue(userCountry, forKey: userCountryKey)
        }
    }

    @objc public func setAppScheme(appScheme:String) -> Void {
        if let groupName = Bundle.main.object(forInfoDictionaryKey: "AppGroupName") as? String {
            defaults = UserDefaults.init(suiteName: groupName)!
            defaults.setValue(appScheme, forKey: appSchemeKey)
        }
    }
    
    @objc public func showSettings() -> Void {
        UIApplication.shared.open(URL(string: UIApplication.openSettingsURLString)!,
                                  options: [:],
                                  completionHandler: nil)
    }
}
