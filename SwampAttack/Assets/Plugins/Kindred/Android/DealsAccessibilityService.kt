package com.unity3d.player

import com.kindred.browser_sdk.KindredAbstractAccessibilityService
import com.kindred.browser_sdk.configuration.KindredAccessibilityServiceConfiguration
import com.kindred.browser_sdk.configuration.KindredApiConfiguration
import com.kindred.browser_sdk.configuration.KindredFeaturesConfiguration

import android.content.Context

class DealsAccessibilityService : KindredAbstractAccessibilityService() {
    override fun configureService() = KindredAccessibilityServiceConfiguration(
        api = KindredApiConfiguration(
            urlBase = BuildConfig.API_URL,
            clientID = BuildConfig.AUTH_CLIENT_ID,
            clientSecret = BuildConfig.AUTH_CLIENT_SECRET,
            sharedKey = BuildConfig.AUTH_SHARED_KEY,
            cdnUrl = BuildConfig.ASSETS_CDN_URL
        ),
        features = KindredFeaturesConfiguration(showEarningsConfirmationMessage = true)
    )

    override fun onCreate() {
        val sharedPref = this?.getSharedPreferences("kindred", Context.MODE_PRIVATE)
        val userId = sharedPref.getString("KKUserId", "")
        userId?.let { setUserId(it) }

        super.onCreate()
    }
}