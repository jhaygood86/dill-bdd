#!/bin/bash

baseDir="$1"
configuration="$2"
outDir="$3"
artifactDirectory="$outDir/drop"

echo "baseDir:" $baseDir
echo "configuration:" $configuration
echo "outDir:" $outDir
echo "artifactDirectory": $artifactDirectory

fixPermissions() {
    chown -R 1000:1000 $outDir $baseDir
}

cleanup() {
	if [ -d $webAppPath/node_modules ] ; then
		rm -r $webAppPath/node_modules
	fi
}

bye() {
	result=$?
	if [ "$result" != "0" ]; then
        echo "Build failed"
        cleanup
	fi
	fixPermissions
	exit $result
}

buildNugetPackage() {
	appPath=$1
	appDirectory=$(dirname ${appPath})
	echo "appDirectory:" $appDirectory
	dotnet restore $appPath
	dotnet pack -c $configuration -o "$outDir/$2" --include-symbols $appPath
}

# stop execution on any error
trap "bye" EXIT
set -e

# build apps
echo "Building Dill"
buildNugetPackage "$baseDir/Dill/Dill.csproj"

# fix permissions on new files as otherwise only root will be able to access them
fixPermissions