from flask import Flask, jsonify, request
import agentpy as ap
import ast
from owlready2 import *

app = Flask(__name__)


## Agent Logic

class CleanbotAgent(ap.Agent):

    #Razonamiento

    def see(self, e):
        e = '[' + e + ']'
        self.per = e
        self.per = list(ast.literal_eval(e))
        print(self.per)
        return self.per

    def next(self, per):
        object_key = [key for key, value in per if value == 'Object']
        bot_key = [key for key, value in per if value == 'Bot']
        wall_key = [key for key, value in per if value == 'Wall']
        shelf_key = [key for key, value in per if value == 'Shelf']

        for rule in self.rules:
            act = rule(per)
            if act is not None:
                return act
        return None

    #DA RULES

    #Rule 1: if I have an object and I'm in from of a shelf, I drop the object
    def rule_1(self, per):
        validator = [False, False]
        shelf_key = [key for key, value in per if value == 'Shelf']

        if self.inventory == True:
            validator[0] = True
        for d in shelf_key:
            if d == self.direction:
                validator[1] = True

        if sum(validator) == 2:
            self.inventory = 0
            return "drop"
        pass

    #Rule 2: if I have an object and there's a shelf nearby and I'm not looking at the shelf's direction, I turn to the right
    def rule_2(self, per):
        validator = [False, False, False]
        shelf_key = [key for key, value in per if value == 'Shelf']

        if self.inventory == True:
            validator[0] = True
        if shelf_key:
            validator[1] = True
        for d in shelf_key:
            if d != self.direction:
                validator[2] = True

        if sum(validator) == 3:
            return "turn"
        pass

    #Rule 3: if I have an object and there's not a shelf nearby and there's something in front of me, I turn to the right
    def rule_3(self, per):
        validator = [False, False, False]
        shelf_key = [key for key, value in per if value == 'Shelf']
        object_key = [key for key, value in per if value == 'Object']
        wall_key = [key for key, value in per if value == 'Wall']

        if self.inventory == True:
            validator[0] = True
        if not shelf_key:
            validator[1] = True
        if self.direction in object_key or self.direction in wall_key:
            validator[2] = True

        if sum(validator) == 3:
            return "turn"
        pass

    #Rule 4: if I have an object and there's not a shelf nearby and there's nothing in front of me, I move forward
    def rule_4(self, per):
        validator = [False, False, False]
        shelf_key = [key for key, value in per if value == 'Shelf']
        object_key = [key for key, value in per if value == 'Object']
        wall_key = [key for key, value in per if value == 'Wall']

        if self.inventory == True:
            validator[0] = True
        if not shelf_key:
            validator[1] = True
        if self.direction not in object_key and self.direction not in wall_key:
            validator[2] = True

        if sum(validator) == 3:
            return "move"
        pass

    #Rule 5: if I don't have an object and I see an object nearby and it's in front of me I grab it
    def rule_5(self, per):
        validator = [False, False, False]
        object_key = [key for key, value in per if value == 'Object']

        if self.inventory == False:
            validator[0] = True
        if object_key:
            validator[1] = True
        for d in object_key:
            if d == self.direction:
                validator[2] = True
        if sum(validator) == 3:
            return "grab"

    #Rule 6: if I don't have an object and I see an object nearby and it's not in front of me I turn to the right
    def rule_6(self, per):
        validator = [False, False, False]
        object_key = [key for key, value in per if value == 'Object']

        if self.inventory == False:
            validator[0] = True
        if object_key:
            validator[1] = True
        for d in object_key:
            if d != self.direction:
                validator[2] = True

        if sum(validator) == 3:
            return "turn"

    #Rule 7: if I see an object nearby and it's not in front of me I turn towards it
    def rule_7(self, per):
        validator = [False, False, False, False]
        object_key = [key for key, value in per if value == 'Object']
        wall_key = [key for key, value in per if value == 'Wall']
        shelf_key = [key for key, value in per if value == 'Shelf']

        if self.inventory == False:
            validator[0] = True
        if not object_key:
            validator[1] = True
        if self.direction not in wall_key:
            validator[2] = True
        if self.direction not in shelf_key:
            validator[3] = True

        if sum(validator) == 4:
            return "move"
        return None

    #Rule 8: if I don't have an object and I don't see an object nearby and I have a wall in front of me or a shelf in front of me, I turn
    def rule_8(self, per):
        validator = [False, False, False]
        object_key = [key for key, value in per if value == 'Object']
        wall_key = [key for key, value in per if value == 'Wall']
        shelf_key = [key for key, value in per if value == 'Shelf']

        if self.inventory == False:
            validator[0] = True
        if not object_key:
            validator[1] = True
        if self.direction in wall_key or self.direction in shelf_key:
            validator[2] = True

        if sum(validator) == 3:
            return "turn"
        return None


    #Simulación de Agente

    def setup(self):
        self.agentType = 0
        self.direction = "N"
        self.inventory = False

        self.rules = (
            self.rule_1,
            self.rule_2,
            self.rule_3,
            self.rule_4,
            self.rule_5,
            self.rule_6,
            self.rule_7,
            self.rule_8
        )



    def step(self, e, inv, dire):
        self.per = self.see(e)
        self.inventory = inv
        self.direction = dire
        self.act = self.next(self.per)
        if self.act is not None:
            print(self.act)
            return self.act
        return None
    
    #Actions 

    # Define the action methods
    def move(self):
        print("Moving forward")
        # Add logic for moving

    def turn(self):
        print("Turning right")
        # Add logic for turning

    def grab(self):
        print("Grabbing object")
        self.inventory = True

    def drop(self):
        print("Dropping object")
        self.inventory = False


class WarehouseModel(ap.Model):
    def setup(self):
        self.cleanbots = ap.AgentList(self, self.p.cleanbots, CleanbotAgent)

    def step(self):
        for bot in self.cleanbots:
            action = bot.step(self.p.environment, self.p.inventory, self.p.direction)
            if action:
                getattr(bot, action)()  # Call the action method

    def update(self):
        pass

    def end(self):
        pass



@app.route("/")
def index():
    return jsonify(200)

@app.route("/receiver", methods=['POST'])
def receiver():
    data = request.json
    print("Received data:", data)
    parameters = {
        'steps': 1,
        'cleanbots': 1,
        'environment': data["resultTuple"],
        #'environment': '"N":"0", "S":"0", "W":"0", "E":"0"',
        'inventory': data["hasObject"],
        'direction': data["rotation"],
    }

    model = WarehouseModel(parameters)
    results = model.run()
    
    # Get the last action performed by the agent
    last_action = model.cleanbots[0].act if model.cleanbots else None

    return (last_action)

if __name__ == "__main__":
    app.run(debug=True)
