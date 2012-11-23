web.config changes

1) 

 <system.webServer>
    <handlers>
      <remove name="StaticHandler" />
      <add name="StaticHandler" path="*.css" verb="*" type="System.Web.StaticFileHandler" modules="ManagedPipelineHandler" resourceType="Unspecified" />

