//
//  KindredSettingsService.swift
//  CashbackExtension
//
//  Created by Shems Boukhatem on 23/12/2021.
//

import Foundation

class KindredSettings {
    static let shared = KindredSettings()

    private static var infoDict: [String: Any] {
        if let dict = Bundle.main.infoDictionary {
            return dict
        }

        fatalError("Info Plist file not found")
    }

    var sharedSettings: UserDefaults

    init() {
        let groupName = Bundle.main.object(forInfoDictionaryKey: "AppGroupName") as? String
        sharedSettings =  UserDefaults(suiteName: groupName) ?? UserDefaults.standard
    }

    func saveSetting(obj: Any, key: String) {
        sharedSettings.set(obj, forKey: key)
        sharedSettings.synchronize()
    }

    func getSetting(key: String, defaultValue: String? = nil) -> String {
        return (sharedSettings.value(forKey: key) as? String) ?? defaultValue ?? ""
    }

    func getSettingBool(key: String, defaultValue: Bool = false) -> Bool {
        return (sharedSettings.value(forKey: key) as? Bool) ?? defaultValue
    }

    func getSettingInt(key: String, defaultValue: Int) -> Int {
        return (sharedSettings.value(forKey: key) as? Int) ?? defaultValue
    }

    func getSettingArray(key: String) -> [String] {
        return (sharedSettings.value(forKey: key) as? [String]) ?? Array()
    }

    func removeSettings(key: String) {
        sharedSettings.removeObject(forKey: key)
        sharedSettings.synchronize()
    }

    func isValueSet(key: String) -> Bool {
        return sharedSettings.value(forKey: key) != nil
    }

    private static let KindredPlistSettings = infoDict[InfoPlistKey.kindredKey] as! [String: Any]
    static let AuthClientId = KindredPlistSettings[InfoPlistKey.ClientIdKey] as! String
    static let AuthClientSecret = KindredPlistSettings[InfoPlistKey.ClientSecretKey] as! String
    static let AuthSharedKey = KindredPlistSettings[InfoPlistKey.SharedKeyKey] as! String
}

enum InfoPlistKey {
    static let kindredKey = "Kindred"
    static let ClientIdKey = "CLIENT_ID"
    static let ClientSecretKey = "CLIENT_SECRET"
    static let SharedKeyKey = "SHARED_KEY"
}

enum MessageType {
    static let authConfig = "authConfig"
    static let sdkAuthConfig = "sdkAuthConfig"
    static let sdkUserConfig = "sdkUserConfig"
    static let setAuthConfig = "setAuthConfig"
    static let getSchemeConfig = "getSchemeConfig"
}

enum DataKey {
    static let accessTokenKey = "accessToken"
    static let refreshTokenKey = "refreshToken"
    static let KKUserIdKey = "KKUserId"
    static let KKUserCurrencyKey = "KKUserCurrency"
    static let KKUserCountryKey = "KKUserCountry"
    static let KKAppSchemeKey = "KKAppScheme"
}
