1. Add web.config section and handler
<sectionGroup name="DocumentViewerSectionGroup">
      <section name="DocumentViewer" type="MCS.DoconutMVC.Helpers.ConfigurationHelper" allowLocation="true" allowDefinition="Everywhere" />
</sectionGroup>
<DocumentViewerSectionGroup>
    <DocumentViewer BasePath="MCS.UI">
    </DocumentViewer>
</DocumentViewerSectionGroup>
<add name="DocImage" verb="GET,POST" path="DocImage.axd" type="DotnetDaddy.DocumentViewer.DocImageHandler, DocumentViewer" />

2. Set session image keys:
SessionInfo.SetObjectInSession(userPreferenceResult.Result.MarkingDoc, "StampImage");
SessionInfo.SetObjectInSession(userPreferenceResult.Result.MessageSignatureDoc, "MessageSignatureImage");
SessionInfo.SetObjectInSession(userPreferenceResult.Result.SignatureDoc, "SignatureImage");
SessionInfo.SetObjectInSession(userPreferenceResult.Result.SignatureDoc, "BarcodeImage");

3. Copy the three reference dlls to the Binaries/Common folder.

