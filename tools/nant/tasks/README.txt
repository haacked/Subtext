NANT TASK FOR YUI COMPRESSOR
----------------------------

AUTHOR
  Aleem Bawany
  http://aleembawany.com/

PROJECT
  http://aleembawany.com/projects/nant-yui-compressor


INSTALLATION
  1. From the zip file copy the following DLLs to the NAnt\bin folder
     - EcmaScript.NET.modified.dll
     - Yahoo.Yui.Compressor.dll
     - aleemb.yui.compress.dll

  2. See usage section for nant build file usage

  (If you are building from source, you need csc.exe
  compiler and nant to build the project).


USAGE
  <yuicompressor todir="${build.dir}">
    <fileset basedir="javascripts">
       <include name="jquery.js" />
       <include name="base.js" />
    </fileset>
  </yuicompressor>
