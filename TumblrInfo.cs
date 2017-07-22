using System;
using DontPanic.TumblrSharp;
using DontPanic.TumblrSharp.Client;

namespace TumblrInfo {
	class TumblrInfo {
		
		const string CONSUMER_KEY = "";
		const string CONSUMER_SECRET = "";
		const string OAUTH_TOKEN = "";
		const string OAUTH_TOKEN_SECRET = "";

		static void Main(string[] args) {
			(new TumblrInfo()).Run();
			Console.ReadLine();
		}

		async private void Run() {
			TumblrClient client = new TumblrClientFactory().Create<TumblrClient>(
				CONSUMER_KEY,
				CONSUMER_SECRET,
				new DontPanic.TumblrSharp.OAuth.Token(OAUTH_TOKEN, OAUTH_TOKEN_SECRET)
			);

			UserInfo userInfo = await client.GetUserInfoAsync();
			Console.WriteLine("Primary Blog: {0}", userInfo.Name);
			Console.WriteLine("Following: {0}", userInfo.FollowingCount);
			Console.WriteLine("Likes: {0}\n", userInfo.LikesCount);

			foreach (UserBlogInfo blog in userInfo.Blogs) {
				BlogInfo blogInfo = null;
				if (blog.BlogType == BlogType.Public) {
					try {
						blogInfo = await client.GetBlogInfoAsync(blog.Name);
					} catch (DontPanic.TumblrSharp.TumblrException e) {
						// well boo =[
					}
				}

				Console.Write("Blog: {0}", blog.Name);
				if (blog.IsPrimary) {
					Console.Write(" [primary]");
				}
				if (blog.BlogType == BlogType.Private) {
					Console.Write(" [private]");
				}
				if ((blogInfo != null) && blogInfo.IsNsfw) {
					Console.WriteLine(" [nsfw]");
				}
				Console.Write("\n-----\n");

				Console.WriteLine("  Followers: {0}", blog.FollowersCount);
				Console.WriteLine("  Posts: {0}", (blogInfo != null) ? blogInfo.PostsCount : 0);

				if (blog.QueueCount > 0) {
					Console.WriteLine("  Queue: {0}", blog.QueueCount);
				}

				if (blog.DraftsCount > 0) {
					Console.WriteLine("  Drafts: {0}", blog.DraftsCount);
				}

				Console.Write("\n");
			}
		}
	}
}
