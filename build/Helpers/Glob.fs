module Glob

open Fake.IO.FileSystemOperators

let fableJs baseDir = baseDir </> "**/*.fs.js"
let fableJsMap baseDir = baseDir </> "**/*.fs.js.map"
let js baseDir = baseDir </> "**/*.js"
let jsMap baseDir = baseDir </> "**/*.js.map"
