## Resource

Resources are serialized runtime data with specific formatted and store in a file.  
Contents are .xnb files.

Resource can depend on multiple orther resources and also depend on Content (ie. which is the lowest dependency in hierachy).

Content may depend on another content, by specify the content file name or the guid and its dependency file must in the same.

## ResourceReader.cs
