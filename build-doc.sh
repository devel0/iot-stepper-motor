#!/bin/bash

exdir="$(dirname `readlink -f "$0"`)"

cd "$exdir"

doxygen

cd "$exdir"/data

doxybook2 -i "$exdir"/xml -o "$exdir"/data/api -c "$exdir"/data/doxybook.json
