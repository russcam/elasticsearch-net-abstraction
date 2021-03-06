using System.IO;
using Elastic.Managed.ConsoleWriters;

namespace Elastic.Managed.Ephemeral.Tasks.AfterNodeStoppedTasks
{
	public class CleanUpDirectoriesAfterNodeStopped : ClusterComposeTask
	{
		public override void Run(IEphemeralCluster<EphemeralClusterConfiguration> cluster)
		{
			var fs = cluster.FileSystem;
			var w = cluster.Writer;
			var v = cluster.ClusterConfiguration.Version;
			DeleteDirectory(w, "cluster data", fs.DataPath);
			DeleteDirectory(w, "cluster config", fs.ConfigPath);
			DeleteDirectory(w, "cluster logs", fs.LogsPath);
			DeleteDirectory(w, "repositories", fs.RepositoryPath);
			var efs = fs as EphemeralFileSystem;
			if (!string.IsNullOrWhiteSpace(efs?.TempFolder))
				DeleteDirectory(w, "cluster temp folder", efs.TempFolder);

			if (efs != null)
			{
				var extractedFolder = Path.Combine(fs.LocalFolder, v.FolderInZip);
				if (extractedFolder != fs.ElasticsearchHome)
				{
					DeleteDirectory(w, "ephemeral ES_HOME", fs.ElasticsearchHome);
				}
			}
		}
		private static void DeleteDirectory(IConsoleLineWriter w, string description, string path)
		{
			if (!Directory.Exists(path)) return;
			w.WriteDiagnostic($"{{{nameof(CleanUpDirectoriesAfterNodeStopped)}}} attempting to delete [{description}]: {{{path}}}");
			Directory.Delete(path, true);
		}

	}
}
