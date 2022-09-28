//
//  KindredSWEHandler.swift
//  CashbackExtension
//
//  Created by Shems Boukhatem on 15/12/2021.
//

import SafariServices
import os.log

class KindredSWEHandler: NSObject, NSExtensionRequestHandling {

    func beginRequest(with context: NSExtensionContext) {
        let item = context.inputItems[0] as! NSExtensionItem
        let message = item.userInfo?[SFExtensionMessageKey] as! Dictionary<String, Any>

        os_log(.default, "Received message: %@", message)

        switch message["type"] as! String {
        case MessageType.authConfig:
            handleAuth(context)
            break
        case MessageType.sdkAuthConfig:
            handleSdkAuth(context)
            break
        case MessageType.sdkUserConfig:
            handleSdkUser(context)
            break
        case MessageType.setAuthConfig:
            handleSetAuth(context, accessToken: message[DataKey.accessTokenKey] as! String, refreshToken: message[DataKey.refreshTokenKey] as! String)
            break
        case MessageType.getSchemeConfig:
            handleGetSchemeConfig(context)
            break
        default:
            break
        }
    }

    func handleAuth(_ context: NSExtensionContext) {
        let response = NSExtensionItem()

        os_log(.default, "Handling Auth")

        let accessToken = KindredSettings.shared.getSetting(key: DataKey.accessTokenKey)
        let refreshToken = KindredSettings.shared.getSetting(key: DataKey.refreshTokenKey)

        response.userInfo = [
            SFExtensionMessageKey: ["type": MessageType.authConfig,
                "accessToken": accessToken,
                "refreshToken": refreshToken]]


        context.completeRequest(returningItems: [response], completionHandler: nil)
    }

    func handleSdkUser(_ context: NSExtensionContext) {
        let response = NSExtensionItem()

        os_log(.default, "Handling SDK User")

        let userId = KindredSettings.shared.getSetting(key: DataKey.KKUserIdKey)
        let userCurrency = KindredSettings.shared.getSetting(key: DataKey.KKUserCurrencyKey)
        let userLocation = KindredSettings.shared.getSetting(key: DataKey.KKUserCountryKey)

        response.userInfo = [
            SFExtensionMessageKey: ["type": MessageType.sdkUserConfig,
                "userId": userId,
                "userCurrency": userCurrency,
                "userLocation": userLocation]]

        context.completeRequest(returningItems: [response], completionHandler: nil)
    }

    func handleSdkAuth(_ context: NSExtensionContext) {
        let response = NSExtensionItem()

        os_log(.default, "Handling SDK Auth")

        let clientId = KindredSettings.AuthClientId
        let clientSecret = KindredSettings.AuthClientSecret
        let sharedKey = KindredSettings.AuthSharedKey

        response.userInfo = [
            SFExtensionMessageKey: ["type": MessageType.sdkAuthConfig,
                "clientId": clientId,
                "clientSecret": clientSecret,
                "sharedKey": sharedKey]]


        context.completeRequest(returningItems: [response], completionHandler: nil)
    }

    func handleSetAuth(_ context: NSExtensionContext, accessToken: String, refreshToken: String) {
        KindredSettings.shared.saveSetting(obj: accessToken, key: DataKey.accessTokenKey)
        KindredSettings.shared.saveSetting(obj: refreshToken, key: DataKey.refreshTokenKey)

        os_log(.default, "Saving new tokens", accessToken, refreshToken)

        context.completeRequest(returningItems: [], completionHandler: nil)
    }

    func handleGetSchemeConfig(_ context: NSExtensionContext) {
        let response = NSExtensionItem()

        let appScheme = KindredSettings.shared.getSetting(key: DataKey.KKAppSchemeKey)

        os_log(.default, "GetAppScheme", appScheme)

        response.userInfo = [
            SFExtensionMessageKey: ["type": "appScheme",
                "scheme": appScheme]]

        context.completeRequest(returningItems: [response], completionHandler: nil)
    }
}
