package com.unity3d.player;

import android.app.Activity;
import android.content.Context;
import android.content.Intent;
import android.content.SharedPreferences;
import android.os.Build;
import android.provider.Settings;
import android.view.inputmethod.InputMethodInfo;
import android.view.inputmethod.InputMethodManager;
import android.net.Uri;

import java.util.ArrayList;
import java.util.List;
import java.io.*;
import java.util.*;

public class KindredSdkBridge {

    public static void setUserId(String uId, Activity currentActivity) {
        SharedPreferences sharedPref = currentActivity.getApplicationContext().getSharedPreferences(
                "kindred", Context.MODE_PRIVATE);
        SharedPreferences.Editor editor = sharedPref.edit();
        editor.putString("KKUserId", uId);
        editor.apply();
    }

    public static void setUserCountry(String uCountry, Activity currentActivity) {
        SharedPreferences sharedPref = currentActivity.getApplicationContext().getSharedPreferences(
                "kindred", Context.MODE_PRIVATE);
        SharedPreferences.Editor editor = sharedPref.edit();
        editor.putString("KKUserCountry", uCountry);
        editor.apply();
    }

    public static void showAccessibilitySettings(Activity currentActivity) {
        currentActivity.startActivity(new Intent(Settings.ACTION_ACCESSIBILITY_SETTINGS));
    }

    public static void showKindredSettings(Activity currentActivity) {
        Intent intent = new Intent(currentActivity, SettingsActivity.class);
        currentActivity.startActivity(intent);
    }

}