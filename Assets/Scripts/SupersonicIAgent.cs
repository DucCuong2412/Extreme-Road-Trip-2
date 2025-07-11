public interface SupersonicIAgent
{
	void start();

	void reportAppStarted();

	void onResume();

	void onPause();

	void setAge(int age);

	void setGender(string gender);

	string getAdvertiserId();

	void validateIntegration();

	void shouldTrackNetworkState(bool track);

	void initRewardedVideo(string appKey, string userId);

	void showRewardedVideo();

	bool isRewardedVideoAvailable();

	void showRewardedVideo(string placementName);

	SupersonicPlacement getPlacementInfo(string name);

	void initInterstitial(string appKey, string userId);

	void showInterstitial();

	bool isInterstitialAdAvailable();

	void initOfferwall(string appKey, string userId);

	void showOfferwall();

	bool isOfferwallAvailable();

	void getOfferwallCredits();
}
