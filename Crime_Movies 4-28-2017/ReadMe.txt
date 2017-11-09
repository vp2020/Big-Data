The scripts need some manual modification to allow display in the GUI

in the R script, just before the plot call (In this example, fancyRpartPlot)
need png(filename=" [output file here] ")
then after the call, you should call dev.off, to close graphics devices/plots

You need a .bat script to handle running the script, like a sort of wrapper

It first changes directory to where you have R installed, to the location of 
Rscript.exe.
Then, it calls Rscript.exe, with the location of the .R script

The C# application launches the .bat file, which then handles the running
of the R script.  The R script saves the resulting plot as a png file.
The C# application then opens and displays the results in the gui window.  The 
button_click event must have the location of the appropriate .bat hard coded in.

After clicking the button, the script is run, the application opens the image,
and releases the handle on it immediately so that it can be used by other scripts,
or other processes.

Naming convention of .bat file - 
File is named for which year it launches evaluation scripts - selecting the 2012 radio button
launches a process running "2012.bat"

The weather R script is just a placeholder, to be replaced by our R scripts.

The application has error checking, and will inform the operator if
	a. They did not select a year to run scripts for
	b. The script file is not found
	c. The results image is not found