# SVT Robotics - Take Home Recruiting Assessment

### Instructions
1. Clone repository to your machine.
2. Using Visual Studio, run the solution.
3. In your browser go to **'https://localhost:5001/swagger/index.html'**
4. A swagger UI has been set up to interact with the API that was created
    I. Select the API call POST api/robots/closest
    II. Click the button **'Try it out'**
    III. In the box below you can edit the payload to be whatever you want
    
    
    
    

```
{
    loadId: 231, //Arbitrary ID of the load which needs to be moved.
    x: 5, //Current x coordinate of the load which needs to be moved.
    y: 3 //Current y coordinate of the load which needs to be moved.
}
```

   IV. After you edit the payload, press execute and you will see the API output below.

### What next?

To improve this API, I would create a few additional API calls to start based off the information provided from the list of robots.
1. An API call for all of the dead robots in the list.  This would return the robotId, and its x and y coordinates.
2. An API call for robots with a battery level below an inputed amount.  This would return the robotId, and its x and y coordinates. (You could also have a charging station for these robots and create an equation for seeing how far away a robot is and making sure it does not go to far away to not make it back to the charging platform.)
3. An API call returning the most charged robots could be useful for determining which robot should be used to retrieve something from far away.
4. A nice to have feature would be a visualization tool taking the list of robots and visually locating them on a grid.  On top of this you could select a robot and it would display its robotId.
