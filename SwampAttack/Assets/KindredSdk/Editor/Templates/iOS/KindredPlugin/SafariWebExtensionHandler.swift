//
//  SafariWebExtensionHandler.swift
//  CashbackExtension
//
//  Created by Shems Boukhatem on 15/12/2021.
//

import SafariServices
import os.log

class SafariWebExtensionHandler: KindredSWEHandler {
    
    var defaults = UserDefaults()

    override init() {
        super.init()
        
        var userId = UUID().uuidString
        let locale = Locale.current
        let userCurrency = locale.currencyCode ?? "USD"
        var userCountry = locale.regionCode ?? "US"
        var appScheme = "kindredsafariextensionsampleapp"
        if let groupName = Bundle.main.object(forInfoDictionaryKey: "AppGroupName") as? String {
            defaults = UserDefaults.init(suiteName: groupName)!
            
            if let defaultUser = defaults.value(forKey: DataKey.KKUserIdKey) as? String {
                userId = defaultUser
            }
            
            if let defaultCountry = defaults.value(forKey: DataKey.KKUserCountryKey) as? String {
                userCountry = defaultCountry
            }
            
            if let defaultAppScheme = defaults.value(forKey: DataKey.KKAppSchemeKey) as? String {
                appScheme = defaultAppScheme
            }
        }

        KindredSettings.shared.saveSetting(obj: userId, key: DataKey.KKUserIdKey)
        KindredSettings.shared.saveSetting(obj: userCurrency, key: DataKey.KKUserCurrencyKey)
        KindredSettings.shared.saveSetting(obj: userCountry, key: DataKey.KKUserCountryKey)
        KindredSettings.shared.saveSetting(obj: appScheme, key: DataKey.KKAppSchemeKey)
    }

}
