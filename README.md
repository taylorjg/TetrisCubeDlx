
## Description

This repo is another [DlxLib](https://github.com/taylorjg/DlxLib) demo program.

![Photo](https://raw.github.com/taylorjg/TetrisCubeDlx/master/Images/Photo.jpg)

## Screenshots

![ScreenshotOfConsoleApp](https://raw.github.com/taylorjg/TetrisCubeDlx/master/Images/ScreenshotOfConsoleApp.png)

## TODO

### WPF Application

* ~~Get basic wireframe of the cube working~~
* Add C# code to draw a (hardcoded) unit cube at 0,0,0
* Fiddle with normals etc to make it look reasonably nice
* Parameterize the above code to draw a unit cube at x,y,z
* Enhance the code to draw a collection of unit cubes
* Enhance the code to take a PlacedPiece
* Enhance the code to draw the PlacedPiece with the correct colour
* Enhance the code to tag a model with a PlacedPiece
* Add code to remove a PlacedPiece
* ~~Add code to solve the puzzle on a background thread~~
    * ~~bounce step/solution events back to the UI thread~~
    * ~~enqueue these events~~
    * ~~add a timer to process enqueued events~~
    * ~~start timer if event is received and timer not currently started~~
    * ~~stop timer when solution event is received and timer is currently started~~
* ~~Add cancellation support (of the solving process)~~
    * ~~invoke cancellation on close of the application (if still solving)~~
* Add code to render a particular set of PlacedPieces
    * leave alone PlacedPieces that are being rendered in the correct place
    * remove PlacedPieces that are being rendered in the wrong place
    * add PlacedPieces that are not being rendered at all
* Make shading etc look nice
* Allow camera position to be altered via sliders
* Allow camera position to be altered via mouse dragging (and key modifiers?)
* Allow zoom to be altered

## Links

* [Tetris puzzle cube](http://www.debenhams.com/webapp/wcs/stores/servlet/prod_10701_10001_106010560399_-1)
* [DlxLib](https://github.com/taylorjg/DlxLib)
