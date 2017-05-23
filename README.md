# GopherServer
A Gopher Server implemented in C#.

## What is in here
 - Simple Socket Server implemented in GopherServer.Server
 - Concept of "Providers' which return a Result for the selector. 
   - Providers only have two methods to implement: `void Init()` and `BaseResult GetResult(string selector);`
 - Models for results, item types, etc and helpers in GopherServer.Core
 - Providers configured in app.config
 - First provider has been written which allows a WordPress site to be viewed in a Gopher client

Pull requests very welcome!

# Providers

## WordPressProvider
The WordPressProvider consumes the WordPress REST API and allows a blog to be browsed with a Gopher client(!).
To use it:
  - Set `<add key="providerName" value="GopherServer.Providers.WpJson" />` 
  - and  adjust `<add key="WordPressProvider.Url" value="<your url>" />` to your URL
 
The provider has support for proxying GIF and Webpages over gopher. 
Give it a go!

# TODO
 - Write Much better documentation
 - Improve the HTML to Text rendering
 - Implement a File provider to expose a directory over gopher (like a typical gopher server would)
 - Improve the socket server (it's pretty much microsoft's example async socket server as is, lots of room for improvement)
 - Improve the configuration and loading of providers
 - Write a client
 - Replace hacky code




