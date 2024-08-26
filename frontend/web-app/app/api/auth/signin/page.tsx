import EmptyFilter from '@/app/auctions/componenets/EmptyFilter'
import React from 'react'

export default function SignIn({searchParams} : {searchParams : {callBackUrl: string}} ) {
  return (
    <EmptyFilter 
    title='You need to be logged in to do that'
    subtitle='Please click below to login'
    showLogin
    callbackUrl={searchParams.callBackUrl}
    />
  )
}
