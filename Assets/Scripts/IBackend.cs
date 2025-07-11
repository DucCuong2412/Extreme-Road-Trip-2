using System.Collections.Generic;

public interface IBackend
{
	void Authenticate();

	bool IsLoggedIn();

	string PlayerAlias();

	string PlayerIdentifier();

	void RetrieveFriends();

	void RegisterRetrieveFriends();

	void UnregisterRetrieveFriends();

	List<User> Friends();

	int GetFriendCount();
}
