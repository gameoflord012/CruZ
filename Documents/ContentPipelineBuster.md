# Content Pipeline Buster

## How assets are built into .xnb

### Import phase

Importer classes reside in topmost folder of ***MonoGame.Framework.Content.Pipeline*** project and they are derived from ***ContentImporter.cs*** with ***ContentImporterAttribute.cs***.

Assets can be from various format, for example texture can be ***.png .jpg*** so called unimported assets.  The ***ContentImporter*** will converter unimported one to immediate object so call ***Unprocess Content***. Example ***TextureContent.cs*** and ***AudioContent*** are class represent unprocess content. So it is just a runtime representation of an asset data reading from file.

### Process phase

After importing phase, unprocess content get process with the appropriate processor. The default content processor is defined in ***ContentImporterAttribute.DefaultProcessor***. The processor is inherited from ***ContentProcessor.cs*** with ***ContentProcessorAttribute.cs***.

Before procsesing resource, a processor can accept many arguments for different settings. For example, setting for texture mipmap generation or quality of the audio.

### Write phase

The process resource then write into file with a writer inherit from ***BuiltInContentWriter***.

## How assets are read from .xnb

By a reader inherit from ***ContentTypeReader.cs***.
