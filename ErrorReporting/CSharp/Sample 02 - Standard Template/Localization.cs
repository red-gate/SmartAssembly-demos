using System;

namespace SmartAssembly.SmartExceptionsWithUI
{
	internal class Localization
	{
#if DEFAULT
		public const int Width = 419;
		public const int Height = 264;

		public const int DebugButtonWidth = 64;
		public const int SendReportButtonWidth = 105;
		public const int DontSendButtonWidth = 75;
		public const int OKRetryButtonWidth = 80;
		public const int CancelButtonWidth = 80;
		public const int ContinueCheckBoxWidth = 226;

		public const int ContinueButtonWidth = 100;
		public const int QuitButtonWidth = 100;

		public const string CannotAttachApp = "{0} cannot initialize itself because some permissions are not granted.\n\nYou probably try to launch {0} in a partial-trust situation. It's usually the case when the application is hosted on a network share.\n\nYou need to run {0} in full-trust, or at least grant it the UnmanagedCode security permission.\n\nTo grant this application the required permission, contact your system administrator, or use the Microsoft .NET Framework Configuration tool.";

		public const string SecurityExceptionMessage = "{0} attempted to perform an operation not allowed by the security policy. To grant this application the required permission, contact your system administrator, or use the Microsoft .NET Framework Configuration tool.\n\n";
		public const string ContinueOrQuit = "If you click Continue, the application will ignore this error and attempt to continue. If you click Quit, the application will close immediately.\n\n";
		public const string SecurityException = "Security error"; //From the CLR : throw new System.Security.SecurityException();
		public const string FatalError = "{0} Unexpected Error"; //{0} #185
		public const string SecurityExceptionHeader = "{0} attempted to perform an operation not allowed by the security policy.";

		public const string CompanyInterestedInLearning = "To help improve the software you use, {0} is interested in learning more about this error. We have created a report about the error for you to send to us."; //#177 modifié
		public const string IgnoreThisError = "Ignore this error and attempt to &continue."; //From the CLR : throw new System.Security.SecurityException(); (modifié)
		public const string PleaseTellCompany = "Please tell {0} about this problem."; //#109

		public const string SendReportButton = "&Send Error Report"; //#101
		public const string DontSendButton = "&Don\'t Send"; //#102
		public const string CancelButton = "&Cancel"; //#20
		public const string OKButton = "&OK"; //#128
		public const string RetryButton = "&Retry"; //From MessageBox -> Retry+Cancel
		public const string DebugButton = "De&bug"; //#112
		public const string ContinueButton = "&Continue"; //From the CLR : throw new System.Security.SecurityException();
		public const string QuitButton = "&Quit"; //From the CLR : throw new System.Security.SecurityException();

		public const string AppHasEncounteredAProblem = "{0} has encountered a problem.\nWe are sorry for the inconvenience."; //#92 modifié
		public const string PleaseWaitWhileSending = "Please wait while {0} is sending the report to {1} through the Internet.";

		public const string PreparingReport = "Preparing the error report."; //#138
		public const string ConnectingToServer = "Connecting to server."; //#7
		public const string TransferringReport = "Transferring report."; //#1
		public const string CompletedThankYou = "Error reporting completed. Thank you."; //#174

		public const string ErrorReporting = "Error Reporting"; //#99

#endif

#if US
		public const int Width = 419;
		public const int Height = 264;

		public const int DebugButtonWidth = 64;
		public const int SendReportButtonWidth = 105;
		public const int DontSendButtonWidth = 75;
		public const int OKRetryButtonWidth = 105;
		public const int CancelButtonWidth = 80;
		public const int ContinueCheckBoxWidth = 226;

		public const int ContinueButtonWidth = 100;
		public const int QuitButtonWidth = 100;

		public const string CannotAttachApp = "{0} cannot initialize itself because some permissions are not granted.\n\nYou probably try to launch {0} in a partial-trust situation. It's usually the case when the application is hosted on a network share.\n\nYou need to run {0} in full-trust, or at least grant it the UnmanagedCode security permission.\n\nTo grant this application the required permission, contact your system administrator, or use the Microsoft .NET Framework Configuration tool.";

		public const string SecurityExceptionMessage = "{0} attempted to perform an operation not allowed by the security policy. To grant this application the required permission, contact your system administrator, or use the Microsoft .NET Framework Configuration tool.\n\n";
		public const string ContinueOrQuit = "If you click Continue, the application will ignore this error and attempt to continue. If you click Quit, the application will close immediately.\n\n";
		public const string SecurityException = "Security error";
		public const string FatalError = "{0} Unexpected Error";
		public const string SecurityExceptionHeader = "{0} attempted to perform an operation not allowed by the security policy.";

		public const string CompanyInterestedInLearning = "To help improve the software you use, {0} is interested in learning more about this error. We have created a report about the error for you to send to us.";
		public const string IgnoreThisError = "Ignore this error and attempt to &continue.";
		public const string PleaseTellCompany = "Please tell {0} about this problem.";

		public const string SendReportButton = "&Send Error Report";
		public const string DontSendButton = "&Don\'t Send";
		public const string CancelButton = "&Cancel";
        public const string OKButton = "&OK";
        public const string RetryButton = "&Retry";
        public const string SaveReportButton = "Save Report";
		public const string DebugButton = "De&bug";
		public const string ContinueButton = "&Continue";
		public const string QuitButton = "&Quit";

		public const string AppHasEncounteredAProblem = "{0} has encountered a problem.\nWe are sorry for the inconvenience.";
		public const string PleaseWaitWhileSending = "Please wait while {0} is sending the report to {1} through the Internet.";

		public const string PreparingReport = "Preparing the error report.";
		public const string ConnectingToServer = "Connecting to server.";
		public const string TransferringReport = "Transferring report.";
		public const string CompletedThankYou = "Error reporting completed. Thank you.";

		public const string ErrorReporting = "Error Reporting";
#endif

#if FR
		public const int Width = 419;
		public const int Height = 264;

		public const int DebugButtonWidth = 74;
		public const int SendReportButtonWidth = 150;
		public const int DontSendButtonWidth = 90;
		public const int OKRetryButtonWidth = 115;
		public const int CancelButtonWidth = 90;
		public const int ContinueCheckBoxWidth = 240;

		public const int ContinueButtonWidth = 100;
		public const int QuitButtonWidth = 100;

		public const string CannotAttachApp = "{0} ne peut pas s'initialiser car certaines permissions ne sont pas accordées.\n\nVous essayez probablement d'exécuter {0} en mode \"partial-trust\". C'est normalement le cas quand l'application est installée sur un réseau.\n\nVous devez lancer {0} en mode \"full-trust\", ou au minimum autoriser l'exécution de code non-managé.\n\nPour accorder à cette application les autorisations requises, contactez votre administrateur système ou utilisez l'outil d'administration de stratégie de sécurité de Microsoft .NET.";

		public const string SecurityExceptionMessage = "{0} a essayé d'effectuer une opération qui est interdite par la stratégie de sécurité. Pour accorder à cette application les autorisations requises, contactez votre administrateur système ou utilisez l'outil d'administration de stratégie de sécurité de Microsoft .NET.\n\n";
		public const string ContinueOrQuit = "Si vous cliquez sur Continuer, l'application ignorera cette erreur et essaiera de continuer. Si vous cliquez sur Quitter, l'application s'arrêtera immédiatement.\n\n";
		public const string SecurityException = "Erreur de sécurité.";
		public const string FatalError = "{0} Erreur inattendue";
		public const string SecurityExceptionHeader = "{0} a essayé d'effectuer une opération qui est interdite par la stratégie de sécurité.";

		public const string CompanyInterestedInLearning = "Afin d'améliorer le logiciel que vous utilisez, {0} souhaiterait obtenir plus d'informations sur cette erreur. Nous avons créé ce rapport dans ce but.";
		public const string IgnoreThisError = "Ignorer cette erreur et essayer de &continuer.";
		public const string PleaseTellCompany = "Veuillez signaler ce problème à {0}.";

		public const string SendReportButton = "Envo&yer le rapport d'erreurs";
		public const string DontSendButton = "Ne pas en&voyer";
		public const string CancelButton = "&Annuler";
		public const string OKButton = "&OK";
		public const string RetryButton = "&Recommencer";
        public const string SaveReportButton = "Enregistrer le rapport";
		public const string DebugButton = "Dé&bogage";
		public const string ContinueButton = "&Continuer";
		public const string QuitButton = "&Quitter";

		public const string AppHasEncounteredAProblem = "{0} a rencontré un problème. Nous vous prions de nous excuser pour le désagrément encouru.";
		public const string PleaseWaitWhileSending = "Veuillez patienter pendant que {0} envoie le rapport à {1} via Internet.";

		public const string PreparingReport = "Préparation du rapport d'erreurs.";
		public const string ConnectingToServer = "Connexion au serveur.";
		public const string TransferringReport = "Transfert du rapport.";
		public const string CompletedThankYou = "Rapport terminé. Merci.";

		public const string ErrorReporting = "Signalement d'erreurs";
#endif

#if DE
		public const int Width = 439;
		public const int Height = 264;

		public const int DebugButtonWidth = 70;
		public const int SendReportButtonWidth = 190;
		public const int DontSendButtonWidth = 90;
		public const int OKRetryButtonWidth =125;
		public const int CancelButtonWidth = 90;
		public const int ContinueCheckBoxWidth = 280;

		public const int ContinueButtonWidth = 100;
		public const int QuitButtonWidth = 100;

		public const string CannotAttachApp = "{0} versuchte einen Vorgang auszuüben, der von der Sicherheitsrichtlinie nicht zugelassen ist.\n\n{0} kann sich nicht initialisieren, weil bestimmte Genehmigungen nicht gewährt werden.\nSie versuchen wahrscheinlich, {0} in Methode \"partial-trust\" auszuführen. Es ist normalerweise der Fall, wenn die Anwendung auf einem Netzwerk installiert ist.\n\nSie müssen {0} in Methode \"full-trust\" starten oder mindestens die Unmanaged Code Ausführung erlauben.\n\nWenden Sie sich an Ihrem Systemadministrator, um dieser Anwendung die erforderliche Berechtigung zu gewähren, oder verwenden Sie das Microsoft .NET-Verwaltungsprogramm für Sicherheitsrechtlinien.";

		public const string SecurityExceptionMessage = "{0} versuchte einen Vorgang auszuüben, der von der Sicherheitsrichtlinie nicht zugelassen ist. Wenden Sie sich an den Systemadministrator. um der Anwendung die erforderliche Berechtigung zu gewähren, oder verwenden Sie das Microsoft .NET-Verwaltungsprogramm für Sicherheitsrichtlinien.\n\n";
		public const string ContinueOrQuit = "Klicken Sie auf \"Weiter\", um den Fehler zu ignorieren und den Vorgang fortzusetzen. Wenn Sie auf \"Beenden\" klicken. wird die Anwendung sofort beendet.\n\n";	
		public const string SecurityException = "Sicherheitsfehler";
		public const string FatalError = "{0} Unerwarteter Fehler";
		public const string SecurityExceptionHeader = "{0} versuchte einen Vorgang auszuüben, der von der Sicherheitsrichtlinie nicht zugelassen ist.";

		public const string CompanyInterestedInLearning = "Um die Software weiterhin zu verbessern, ist {0} daran interessiert, weitere Einzelheiten über den Fehler in Erfahrung zu bringen. Wir haben ein Bericht bezüglich dem Fehler für Ihre Weiterleitung an uns erstellt.";
		public const string IgnoreThisError = "&Ignorieren und versuchen den Vorgang fortzusetzen";
		public const string PleaseTellCompany = "Informieren Sie {0} über dieses Problem.";

		public const string SendReportButton = "Diese &Fehlerberichterstattung senden";
		public const string DontSendButton = "&Nicht berichten";
		public const string CancelButton = "&Abbrechen";
		public const string OKButton = "&OK";
		public const string RetryButton = "Wieder&holen";
        public const string SaveReportButton = "Fehlerberich speichern";
		public const string DebugButton = "&Debuggen";

		public const string ContinueButton = "&Weiter";
		public const string QuitButton = "&Beenden";

		public const string AppHasEncounteredAProblem = "{0} hat ein Problem festgestellt.";
		public const string PleaseWaitWhileSending = "Gedulden Sie sich bitte, während {0} den Fehlerbericht an {1} via Internet sendet.";

		public const string PreparingReport = "Fehlerberichterstattung wird vorbereitet.";
		public const string ConnectingToServer = "Serververbindung.";
		public const string TransferringReport = "Bericht wird übertragen.";
		public const string CompletedThankYou = "Berichterstattung abgeschlossen. Vielen Dank.";

		public const string ErrorReporting = "Fehlerberichterstattung";
#endif

    }
}
