## Resource

Resources are serialized runtime data with specific formatted and store in a file.  
Contents are .xnb files.

Resource can depend on multiple orther resources and also depend on Content (ie. which is the lowest dependency in hierachy).

To import the resource in to memmory, the process as follow:

1. Read the resource file and put it in immediate object
2. The immediate object is now loaded into empty instance