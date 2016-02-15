from kivy.uix.floatlayout import FloatLayout

__author__ = 'Marnee Dearman'
from kivy.app import App
from kivy.uix.widget import Widget
from kivy.properties import NumericProperty, ReferenceListProperty,\
    ObjectProperty
from kivy.vector import Vector
from kivy.clock import Clock
from random import randint

class StartButton(Widget):
    pass

class PongPaddle(Widget):
    score = NumericProperty(0)

    def bounce_ball(self, ball):
        if self.collide_widget(ball):
            vx, vy = ball.velocity
            offset = (ball.center_y - self.center_y) / (self.height / 2)
            bounced = Vector(-1 * vx, vy)
            vel = bounced * 1.1
            ball.velocity = vel.x, vel.y + offset


class PongBall(Widget):
    velocity_x = NumericProperty(0)
    velocity_y = NumericProperty(0)
    velocity = ReferenceListProperty(velocity_x, velocity_y)

    def move(self):
        self.pos = Vector(*self.velocity) + self.pos

    def stop(self):
        self.pos = self.parent.center_x - 5, self.parent.center_y
        self.velocity = (0, 0)


class PongGame(Widget):
    ball = ObjectProperty(None)
    player1 = ObjectProperty(None)
    player2 = ObjectProperty(None)
    player1_score = ObjectProperty(None)
    player2_score = ObjectProperty(None)
    winner = ObjectProperty(None)
    start_game = ObjectProperty(None)

    def start(self):
        self.remove_widget(self.start_game)
        self.winner.text = ""
        self.player1_score.text = "0"
        self.player2_score.text = "0"
        self.serve_ball()

    def serve_ball(self, vel=(4, 0)):
        self.ball.center = self.center
        self.ball.velocity = vel

    def update(self, dt):
        self.ball.move()

        # bounce off paddles
        self.player1.bounce_ball(self.ball)
        self.player2.bounce_ball(self.ball)

        # bounce off top and bottom
        if (self.ball.y < 0) or (self.ball.top > self.height):
            self.ball.velocity_y *= -1

        # went off to a side to score point?
        if self.ball.x < self.x:
            self.player2.score += 1
            self.player2_score.text = str(self.player2.score)
            if self.player2.score == 1:
                self.ball.stop()
                self.winner.text = "PLAYER 2 WINS"
                # reset the paddle
                self.add_widget(self.start_game)
                self.player2.center_y = self.center_y
                self.player1.center_y = self.center_y
                self.player1.score = 0
                self.player2.score = 0
            else:
                self.serve_ball(vel=(4, 0))
        if self.ball.x > self.width:
            self.player1.score += 1
            self.player1_score.text = str(self.player1.score)
            if self.player1.score == 1:
                self.ball.stop()
                self.winner.text = "PLAYER 1 WINS"
                self.add_widget(self.start_game)
                self.player1.center_y = self.center_y
                self.player2.center_y = self.center_y
                self.player1.score = 0
                self.player2.score = 0
            else:
                self.serve_ball(vel=(-4, 0))

    def on_touch_move(self, touch):
            if touch.x < self.width / 3:
                self.player1.center_y = touch.y
            if touch.x > self.width - self.width / 3:
                self.player2.center_y = touch.y


class PongApp(App):
    def build(self):
        game = PongGame()

        # start_button = StartButton()
        # start_button.start_button.on_press = game.start_game
        # game.add_widget(start_button)
        # game.serve_ball()
        Clock.schedule_interval(game.update, 1.0/60.0)
        return game

    # def start_game(self):
    #     pass

if __name__ == '__main__':
    PongApp().run()