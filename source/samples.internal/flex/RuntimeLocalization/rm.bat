SET OUT=%1_RM.swf
mxmlc -locale=%1 -source-path=locale/{locale} -include-resource-bundles=R -output=%OUT%
copy %OUT% .\bin