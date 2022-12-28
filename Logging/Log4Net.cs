using System.Reflection;
using System.Xml;
using log4net;
using log4net.Config;

namespace elec.Logging
{
    public class Log4Net<T> : ILogger<T>
    {
        private readonly ILog logger = LogManager.GetLogger(typeof(T));
        public Log4Net(){
            try
            {
                XmlDocument log4netConfig = new XmlDocument();

                using (var fs = File.OpenRead("log4net.config"))
                {
                    log4netConfig.Load(fs);
                    var repo = LogManager.CreateRepository(Assembly.GetEntryAssembly(), typeof(log4net.Repository.Hierarchy.Hierarchy));
                    XmlConfigurator.Configure(repo, log4netConfig["log4net"]);
                    logger.Debug($"log4net Logger for type '{typeof(T)}' initialized.");
                }
            }
            catch
            {
                //TODO: handling logovanie nedostupne
            }
        }
        public void Info(string message){
            logger.Info(message);
        }
        public void Debug(string message){
            logger.Debug(message);
        }
        public void Warn(string message){
            logger.Warn(message);
        }
        public void Error(string message){
            logger.Error(message);
        }
        public void Error(string message, Exception ex){
            logger.Error(message, ex);
        }
    }
}