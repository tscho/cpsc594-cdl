#!/bin/sh

dirs=`find ./ -maxdepth 1 -type d | sed "s/\.\/\///g"`
outputpath="../testcoverage"

for dir in $dirs; do
  mkdir -p $outputpath/$dir
  lcov -c -d $dir -b $dir --output-file $outputpath/$dir/$dir.coverage;
done