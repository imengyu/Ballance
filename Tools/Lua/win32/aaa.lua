

return function ()
    local aaa = {}

    function aaa:getTime()
    return self.a
    end 

    function aaa:setTime(a)
    self.a = a
    end 
    return aaa
end 