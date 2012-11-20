function insert(item, user, request) {    
    item.userId = user.userId;
    
    if (item.speaker) {
        var url = 'https://api.twitter.com/1/users/show.json?screen_name=' + item.speaker;
        var req = require('request');
        
        req.get(url,
        function(error, result, body) {
           item.img = '';  
           if (error || result.statusCode != 200) request.execute();                          
           
           var json = JSON.parse(body);                      
           if(json.profile_image_url){
                var biggerImg =  json.profile_image_url.replace("normal","bigger");
                item.img = biggerImg;    
           }
                           
           request.execute(); 
        }); 
    }
    else {
        item.img = 'Assets/NoProfile.png'
        request.execute(); 
    } 
}