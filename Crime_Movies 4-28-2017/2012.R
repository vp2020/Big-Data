# Rattle is Copyright (c) 2006-2015 Togaware Pty Ltd.

#============================================================
# Rattle timestamp: 2017-04-24 20:54:17 x86_64-w64-mingw32 

# Rattle version 4.1.0 user 'Denver'

# This log file captures all Rattle interactions as R commands. 

# Export this log to a file using the Export button or the Tools 
# menu to save a log of all your activity. This facilitates repeatability. For example, exporting 
# to a file called 'myrf01.R' will allow you to type in the R Console 
# the command source('myrf01.R') and so repeat all actions automatically. 
# Generally, you will want to edit the file to suit your needs. You can also directly 
# edit this current log in place to record additional information before exporting. 
 
# Saving and loading projects also retains this log.

# We begin by loading the required libraries.

library(rattle)   # To access the weather dataset and utility commands.
library(magrittr) # For the %>% and %<>% operators.

# This log generally records the process of building a model. However, with very 
# little effort the log can be used to score a new dataset. The logical variable 
# 'building' is used to toggle between generating transformations, as when building 
# a model, and simply using the transformations, as when scoring a dataset.

building <- TRUE
scoring  <- ! building


# A pre-defined value is used to reset the random seed so that results are repeatable.

crv$seed <- 42 

#============================================================
# Rattle timestamp: 2017-04-24 20:56:39 x86_64-w64-mingw32 

# Load an R dataset.

data(list = "weather", package = "rattle")
crs$dataset <- weather
names(crs$dataset) <- gsub("-", ".", names(crs$dataset))

#============================================================
# Rattle timestamp: 2017-04-24 20:56:41 x86_64-w64-mingw32 

# Note the user selections. 

# Build the training/validate/test datasets.

set.seed(crv$seed) 
crs$nobs <- nrow(crs$dataset) # 366 observations 
crs$sample <- crs$train <- sample(nrow(crs$dataset), 0.7*crs$nobs) # 256 observations
crs$validate <- sample(setdiff(seq_len(nrow(crs$dataset)), crs$train), 0.15*crs$nobs) # 54 observations
crs$test <- setdiff(setdiff(seq_len(nrow(crs$dataset)), crs$train), crs$validate) # 56 observations

# The following variable selections have been noted.

crs$input <- c("Date", "Location", "MinTemp", "MaxTemp",
     "Rainfall", "Evaporation", "Sunshine", "WindGustDir",
     "WindGustSpeed", "WindDir9am", "WindDir3pm", "WindSpeed9am",
     "WindSpeed3pm", "Humidity9am", "Humidity3pm", "Pressure9am",
     "Pressure3pm", "Cloud9am", "Cloud3pm", "Temp9am",
     "Temp3pm", "RainToday")

crs$numeric <- c("MinTemp", "MaxTemp", "Rainfall", "Evaporation",
     "Sunshine", "WindGustSpeed", "WindSpeed9am", "WindSpeed3pm",
     "Humidity9am", "Humidity3pm", "Pressure9am", "Pressure3pm",
     "Cloud9am", "Cloud3pm", "Temp9am", "Temp3pm")

crs$categoric <- c("Location", "WindGustDir", "WindDir9am", "WindDir3pm",
     "RainToday")

crs$target  <- "RainTomorrow"
crs$risk    <- "RISK_MM"
crs$ident   <- NULL
crs$ignore  <- NULL
crs$weights <- NULL

#============================================================
# Rattle timestamp: 2017-04-24 20:57:32 x86_64-w64-mingw32 

# Decision Tree 

# The 'rpart' package provides the 'rpart' function.

library(rpart, quietly=TRUE)

# Reset the random number seed to obtain the same results each time.

set.seed(crv$seed)

# Build the Decision Tree model.

crs$rpart <- rpart(RainTomorrow ~ .,
    data=crs$dataset[crs$train, c(crs$input, crs$target)],
    method="class",
    parms=list(split="information"),
    control=rpart.control(usesurrogate=0, 
        maxsurrogate=0))

# Generate a textual view of the Decision Tree model.

print(crs$rpart)
printcp(crs$rpart)
cat("\n")

# Time taken: 0.18 secs

#============================================================
# Rattle timestamp: 2017-04-24 20:57:40 x86_64-w64-mingw32 

# Plot the resulting Decision Tree. 

# We use the rpart.plot package.

png(filename="C:\\Users\\Denver\\Documents\\COLLEGE\\RIT\\17SPRING\\BigData\\GroupProject\\res.png")
fancyRpartPlot(crs$rpart, main="Decision Tree weather $ RainTomorrow")
dev.off()
