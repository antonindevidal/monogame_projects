# 3D monogame project


![Map and grass generation](https://github.com/antonindevidal/monogame_projects/blob/main/3dtest/images/hardwareInstancing.gif)
*The gif is laggy because it was recorded at 15 fps*

## Map Generation

Generated with simplex noise using [SimplexNoise](https://github.com/WardBenjamin/SimplexNoise) nugget by WardBenjamin 

## Grass Generation 

- Use of hardware instancing to instance millions of grass blade (5 vertices each)
- For the wind effect, use of sine to offset the blade according to the height of the vertex

